namespace Skeleton.Tests.Infrastructure
{
    public class MetadataType
    {
        private int _field;
        public int Property { get; set; }

        public int Method()
        {
            return _field + Property;
        }
    }
}