using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class Reference : EntityBase
    {
        private string myUri;

        [Required, Url]
        public string Url
        {
            get { return myUri; }
            set { SetProperty( ref myUri, value ); }
        }

        [Required]
        public virtual Company Company { get; set; }
    }
}
