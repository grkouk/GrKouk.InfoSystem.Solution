﻿using System;
using System.Collections.Generic;
using System.Text;
using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Diaries
{
    public class DiaryDto
    {
        public string Name { get; set; }
        public DiaryTypeEnum DiaryType { get; set; }
        public string SelectedDocTypes { get; set; }
    }
}
