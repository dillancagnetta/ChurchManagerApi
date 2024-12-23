﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class ChurchAttendanceDbRepository : GenericRepositoryBase<ChurchAttendance>, IChurchAttendanceDbRepository
    {
        public ChurchAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(
            DateTime from, DateTime to, int? churchId = null)
        {
            var query =  Queryable();

            if (churchId.HasValue && churchId.Value > 0)
            {
                query = query.Where(x => x.ChurchId == churchId.Value);
            }
            
                var raw = await query
                .Where(x => x.AttendanceDate >= from && x.AttendanceDate <= to)
                .Select(x => new
                {
                    x.AttendanceDate,
                    x.AttendanceCount,
                    x.NewConvertCount,
                    x.FirstTimerCount,
                })
                .GroupBy(x => new
                    {
                        Year = x.AttendanceDate.Year,
                        Month = x.AttendanceDate.Month
                    },
                    (x, e) => new ChurchAttendanceMonthlyTotalsVm
                    {
                        Year = x.Year,
                        Month = x.Month,
                        TotalAttendance = e.Sum(y => y.AttendanceCount),
                        TotalNewConverts = e.Sum(y => y.NewConvertCount),
                        TotalFirstTimers = e.Sum(y => y.FirstTimerCount),
                    })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();

            return raw
                .GroupBy(x => x.Year)
                .Select( x => new ChurchAttendanceAnnualBreakdownVm
                {
                    Year = x.Key,
                    Data = x
                });
        }

        public async Task<dynamic> DashboardChurchAttendanceBreakdownAsync(DateTime from, DateTime to)
        {
            var raw = await Queryable()
                .Where(x => x.AttendanceDate >= from && x.AttendanceDate <= to)
                .Select(x => new
                {
                    x.AttendanceDate,
                    x.AttendanceCount,
                    x.MalesCount,
                    x.FemalesCount,
                    x.ChildrenCount,
                })
                .GroupBy(x => new
                    {
                        Year = x.AttendanceDate.Year,
                        Month = x.AttendanceDate.Month
                    },
                    (x, e) => new 
                    {
                        Year = x.Year,
                        Month = x.Month,
                        TotalAttendance = e.Sum(y => y.AttendanceCount),
                        TotalMales = e.Sum(y => y.MalesCount),
                        TotalFemales = e.Sum(y => y.FemalesCount),
                        TotalChildren = e.Sum(y => y.ChildrenCount),
                    })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();

            return raw
                .GroupBy(x => x.Year)
                .Select(x => new
                {
                    Year = x.Key,
                    Data = x
                });
        }
    }
}
