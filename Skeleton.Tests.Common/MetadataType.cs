namespace Skeleton.Tests.Common
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

        public int Property
        {
            get; set;
        }

        public int Method()
        {
            return Field + Property;
        }

        public void Method(int value)
        {
            Property = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public int Field;
    }
}