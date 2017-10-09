namespace Web.Infrastructure.DataTables
{
    /// <summary>
    /// Represents a sorted column from DataTables.
    /// </summary>
    public class SortedColumn
    {
        private const string Ascending = "asc";

        public SortedColumn(string propertyName, string sortingDirection)
        {
            PropertyName = propertyName;
            Direction = sortingDirection.Equals(Ascending) ? SortingDirection.Ascending : SortingDirection.Descending;
        }

        /// <summary>
        /// Gets the name of the Property on the class to sort on.
        /// </summary>
        public string PropertyName { get; private set; }

        public SortingDirection Direction { get; private set; }

        public override int GetHashCode()
        {
            var directionHashCode = Direction.GetHashCode();
            return PropertyName != null ? PropertyName.GetHashCode() + directionHashCode : directionHashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SortedColumn)obj;

            if (other.Direction != Direction)
            {
                return false;
            }

            return other.PropertyName == PropertyName;
        }
    }
}