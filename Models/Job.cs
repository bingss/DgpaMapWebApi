using System;
using System.Collections.Generic;

namespace DgpaMapWebApi.Models;

public partial class Job
{
    public Guid JOB_ID { get; set; }

    public string ORG_ID { get; set; }

    public string ORG_NAME { get; set; }

    public int? RANK_START { get; set; }

    public int? RANK_END { get; set; }

    public string TITLE { get; set; }

    public string SYSNAM { get; set; }

    public int? NUMBER_OF { get; set; }

    public int? RESERVE_NUM { get; set; }

    public string WORK_PLACE_TYPE { get; set; }

    public DateOnly? DATE_FROM { get; set; }

    public DateOnly? DATE_TO { get; set; }

    public bool? IS_HANDICAP { get; set; }

    public bool? IS_ORIGINAL { get; set; }

    public bool? IS_LOCAL_ORIGINAL { get; set; }

    public bool? IS_TRANSFER { get; set; }

    public bool? IS_TRANING { get; set; }

    public string WORK_QUALITY { get; set; }

    public string WORK_ITEM { get; set; }

    public string WORK_ADDRESS { get; set; }

    public string CONTACT_METHOD { get; set; }

    public string VIEW_URL { get; set; }

    public decimal? Coordinate_X { get; set; }

    public decimal? Coordinate_Y { get; set; }

    public bool IS_XYDoubt { get; set; }
}
