﻿using System.Collections.Generic;

namespace EOToolsWeb.Control.Grid;

public class PaginatedResultModel<T>
{
    public required IEnumerable<T> Results { get; set; }

    public required int TotalCount { get; set; }
}
