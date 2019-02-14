using GrKouk.InfoSystem.Definitions;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Diaries
{
    public class DiaryDto
    {
        public string Name { get; set; }
        public DiaryTypeEnum DiaryType { get; set; }
        public string SelectedDocTypes { get; set; }
        public string SelectedMatNatures { get; set; }
        
        public string SelectedTransTypes { get; set; }
    }
}
