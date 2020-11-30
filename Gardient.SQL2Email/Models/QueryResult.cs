using System;
using System.Collections.Generic;
using System.Linq;

namespace Gardient.SQL2Email.Models
{
    class QueryResult
    {
        private readonly List<string[]> _rows;

        public string[] Columns { get; private set; }
        public int RowCount => _rows.Count;
        public IEnumerable<QueryResultRow> Rows
        {
            get
            {
                return _rows.Select(GetDictionaryForRow);
            }
        }
        public QueryResult(IEnumerable<string> columnNames)
        {
            _rows = new List<string[]>();
            Columns = columnNames.Select(x => x.ToLower()).ToArray();
        }

        public void AddRow(object[] row, Func<object, string> valueTransformer = null)
        {
            if (valueTransformer == null)
                valueTransformer = x => x.ToString();

            _rows.Add(row.Select(valueTransformer).ToArray());
        }

        private QueryResultRow GetDictionaryForRow(string[] row)
        {
            QueryResultRow dict = new QueryResultRow();
            for (int i = 0; i < Columns.Length; i++)
            {
                dict.Add(Columns[i], row[i]);
            }
            return dict;
        }
    }

    class QueryResultRow : Dictionary<string, string>
    {
        public string Email => this["email"];
        public string ModifiedOn => this["modifiedOn"];
    }
}
