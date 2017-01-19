namespace Skeleton.Tests.Infrastructure
{
    public class MetadataType
    {
        public MetadataType()
        {
        }

        public MetadataType(int value)
        {
            Field = value;
        }

        private int _field;
        public int Property
        {
            get; set;
        }

        public int Method()
        {
            return _field + Property;
        }

        public void Method(int value)
        {
            Property = value;
        }

        public int Field;

        private int _writeOnlyProperty;
        public int WriteOnlyProperty
        {
            set
            {
                _writeOnlyProperty = value;
            }
        }

        private readonly int _readOnlyProperty;
        public int ReadOnlyProperty
        {
            get
            {
                return _readOnlyProperty;
            }
        }
    }
}