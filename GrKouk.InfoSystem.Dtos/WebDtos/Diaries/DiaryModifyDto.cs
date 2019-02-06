using GrKouk.InfoSystem.Domain.FinConfig;

namespace GrKouk.InfoSystem.Dtos.WebDtos.Diaries
{
    public class DiaryModifyDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public DiaryTypeEnum DiaryType { get; set; }
        public string SelectedDocTypes { get; set; }
        public string SelectedMatNatures { get; set; }

        public string SelectedTransTypes { get; set; }
    }
}