#!/usr/bin/env python3
"""
assign_giang_vien.py

Fill `ma_giang_vien` in a schedule CSV using course catalog and teacher list.

Usage:
  python assign_giang_vien.py \
    --schedule "path/to/kq_dkhp.csv" \
    --courses "path/to/danh_muc_mon_hoc.csv" \
    --teachers "path/to/giang_vien.csv" \
    --out "path/to/output.csv"

By default this writes a new file and does not overwrite the original.
Algorithm: for rows where `hinh_thuc_giang_day` is one of {"LT","HT1"},
  find the course's department from `danh_muc_mon_hoc.csv` and assign a
  teacher from `giang_vien.csv` whose `khoa_bo_mon` matches that department.
Teachers are selected round-robin per department. For other teaching forms,
  `ma_giang_vien` is set to "*".

This script is intentionally small and conservative. It logs assignment counts
and writes the output CSV with the same delimiter/columns as the input schedule.
"""
import csv
import argparse
import sys
from collections import defaultdict


def load_teachers(path):
    teachers_by_dept = defaultdict(list)
    with open(path, newline='', encoding='utf-8-sig') as f:
        # giang_vien.csv is comma-separated
        reader = csv.DictReader(f)
        for r in reader:
            ma = r.get('ma_giang_vien') or r.get('ma_gv')
            dept = (r.get('khoa_bo_mon') or '').strip()
            if not ma:
                continue
            ma = ma.strip()
            # ignore placeholder '*' or empty
            if ma == '*' or ma == 'NULL' or ma == '':
                continue
            if dept == '':
                # skip unknown-dept teachers for this algorithm
                continue
            teachers_by_dept[dept].append(ma)
    return teachers_by_dept


def load_courses(path):
    # danh_muc_mon_hoc.csv uses semicolon delimiter
    course_dept = {}
    with open(path, newline='', encoding='utf-8-sig') as f:
        reader = csv.DictReader(f, delimiter=';')
        # header has 'Mã MH' and 'Đơn vị quản lý chuyên môn'
        # try a few plausible column names
        for r in reader:
            code = r.get('Mã MH') or r.get('Ma MH') or r.get('Mã MH;')
            dept = r.get('Đơn vị quản lý chuyên môn') or r.get('Don vi quan ly chuyen mon') or r.get('Đơn vị quản lý chuyên môn')
            if not code:
                # try first field
                keys = list(r.keys())
                if keys:
                    code = r[keys[0]]
            if code:
                code = code.strip()
                if dept:
                    dept = dept.strip()
                else:
                    dept = ''
                course_dept[code] = dept
    return course_dept


def assign(schedule_path, courses_path, teachers_path, out_path, overwrite=False):
    teachers_by_dept = load_teachers(teachers_path)
    course_dept = load_courses(courses_path)

    # counters for round-robin per department
    counters = defaultdict(int)
    assigned_counts = defaultdict(int)
    missing_depts = set()

    with open(schedule_path, newline='', encoding='utf-8-sig') as f_in:
        # schedule uses semicolon delimiter
        reader = csv.DictReader(f_in, delimiter=';')
        fieldnames = reader.fieldnames
        if fieldnames is None:
            print('ERROR: schedule CSV has no header', file=sys.stderr)
            return 1
        rows = list(reader)

    for r in rows:
        hinh = (r.get('hinh_thuc_giang_day') or '').strip()
        # If hinh_thuc not LT or HT1 => '*'
        if hinh not in {'LT', 'HT1'}:
            r['ma_giang_vien'] = '*'
            continue

        ma_mon = (r.get('ma_mon_hoc') or '').strip()
        dept = course_dept.get(ma_mon, '')
        if not dept:
            # missing mapping -> mark '*' and record
            r['ma_giang_vien'] = '*'
            missing_depts.add(ma_mon)
            continue

        available = teachers_by_dept.get(dept, [])
        if not available:
            # no teacher in that department -> mark '*' and record
            r['ma_giang_vien'] = '*'
            missing_depts.add(dept)
            continue

        idx = counters[dept] % len(available)
        ma_gv = available[idx]
        counters[dept] += 1
        r['ma_giang_vien'] = ma_gv
        assigned_counts[ma_gv] += 1

    # write out
    # ensure fieldnames order preserved; if ma_giang_vien missing in header add it
    if 'ma_giang_vien' not in fieldnames:
        fieldnames = fieldnames + ['ma_giang_vien']

    mode = 'w'
    with open(out_path, mode, newline='', encoding='utf-8-sig') as f_out:
        writer = csv.DictWriter(f_out, fieldnames=fieldnames, delimiter=';')
        writer.writeheader()
        for r in rows:
            # ensure all fields present
            out_row = {k: r.get(k, '') for k in fieldnames}
            writer.writerow(out_row)

    # write detailed logs
    base = out_path.rsplit('.csv', 1)[0]
    assignments_log = base + '_assignments.csv'
    teacher_summary = base + '_teacher_summary.csv'
    missing_file = base + '_missing.txt'

    # assignments: list each row and its assigned teacher (or '*') with dept
    with open(assignments_log, 'w', newline='', encoding='utf-8-sig') as f_a:
        cols = ['hoc_ky', 'ma_mon_hoc', 'ma_lop', 'hinh_thuc_giang_day', 'ma_giang_vien', 'dept']
        w = csv.DictWriter(f_a, fieldnames=cols, delimiter=';')
        w.writeheader()
        for r in rows:
            ma_mon = (r.get('ma_mon_hoc') or '').strip()
            dept = course_dept.get(ma_mon, '')
            w.writerow({
                'hoc_ky': r.get('hoc_ky', ''),
                'ma_mon_hoc': ma_mon,
                'ma_lop': r.get('ma_lop', ''),
                'hinh_thuc_giang_day': r.get('hinh_thuc_giang_day', ''),
                'ma_giang_vien': r.get('ma_giang_vien', ''),
                'dept': dept,
            })

    # teacher summary: counts per teacher
    with open(teacher_summary, 'w', newline='', encoding='utf-8-sig') as f_t:
        w = csv.writer(f_t, delimiter=';')
        w.writerow(['ma_giang_vien', 'assigned_count'])
        for ma, cnt in sorted(assigned_counts.items(), key=lambda x: -x[1]):
            w.writerow([ma, cnt])

    # missing mappings/units
    if missing_depts:
        with open(missing_file, 'w', encoding='utf-8-sig') as f_m:
            for item in sorted(missing_depts):
                f_m.write(item + '\n')

    # summary to stdout
    print(f'Wrote {len(rows)} rows to {out_path}')
    total_assigned = sum(assigned_counts.values())
    print(f'Assigned {total_assigned} teacher-allocations across {len(assigned_counts)} teachers')
    print(f'Assignments log: {assignments_log}')
    print(f'Teacher summary: {teacher_summary}')
    if missing_depts:
        print(f'Missing mappings written to: {missing_file}')

    return 0


def main():
    p = argparse.ArgumentParser(description='Assign ma_giang_vien in schedule using catalog and teacher list')
    p.add_argument('--schedule', required=True, help='Path to schedule CSV (semicolon separated)')
    p.add_argument('--courses', required=True, help='Path to danh_muc_mon_hoc.csv (semicolon separated)')
    p.add_argument('--teachers', required=True, help='Path to giang_vien.csv (comma separated)')
    p.add_argument('--out', required=False, help='Output path; default: schedule_with_gv.csv next to schedule')
    p.add_argument('--overwrite', action='store_true', help='Overwrite schedule file (danger)')
    args = p.parse_args()

    out = args.out
    if not out:
        out = args.schedule.replace('.csv', '') + '_with_gv.csv'

    if args.overwrite and out == args.schedule:
        # allow overwrite only if explicitly requested
        pass

    rc = assign(args.schedule, args.courses, args.teachers, out, overwrite=args.overwrite)
    sys.exit(rc)


if __name__ == '__main__':
    main()
