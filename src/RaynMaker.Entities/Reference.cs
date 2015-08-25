using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class Reference : SerializableBindableBase
    {
        private string myUri;

        [Required]
        public long Id { get; set; }

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
