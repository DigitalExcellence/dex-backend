﻿using Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sources
{
    public interface ISource
    {
        Task<IEnumerable<SearchResult>> Search(string searchTerm);
        void getSource(string url);
    }
}