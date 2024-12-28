using System;
using System.Collections.Generic;

namespace DgpaMapWebApi.Models;

public partial class UpdateDate
{
    public DateOnly? LastUpdateDate { get; set; }

    public Guid Date_ID { get; set; }
}
