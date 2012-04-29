namespace Catnap.Find.Conditions
{
    public class Equal : LeftRightCondition
    {
        public Equal(string columnName, object value) : base(columnName, value) { }

        protected override string Operator
        {
            get { return "="; }
        }
    }
}