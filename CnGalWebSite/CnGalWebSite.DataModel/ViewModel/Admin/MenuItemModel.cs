﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CnGalWebSite.DataModel.ViewModel.Admin
{
    public class MenuItemModel
    {
        public string Text { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public List<MenuItemModel> Child { get; set; }
    }
}
