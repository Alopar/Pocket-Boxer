using System;

namespace Services.Database
{
    public abstract class AbstractTableData
    {
        public AbstractTableData Copy()
        {
            return (AbstractTableData)MemberwiseClone();
        }
    }
}
