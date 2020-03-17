using MapKit;

namespace MyCensus.iOS.Renderers
{
    public class CustomMKAnnotationView : MKAnnotationView
    {
        public int Id { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, int id)
            : base(annotation, id.ToString())
        {
        }
    }
}