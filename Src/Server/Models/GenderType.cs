namespace Ccms.Entities.Users
{
    using System.ComponentModel.DataAnnotations;

    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 1,

        [Display(Name = "زن")]
        Female = 2
    }
}
