using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace VectorMap2Opendrive
{
    public class DataFile
    {
        public delegate string LoadFile(string fileName);

        ArrayList m_DataTypeArray;    // first line, data type array
        ArrayList m_DataList;         // all data
        ArrayList m_RowListNow;       // current row


        public DataFile()
        {
            m_DataTypeArray = new ArrayList(16);
            m_DataList = new ArrayList(16);
        }


        // Load and parse data from string buffer
        public bool LoadData(string fileName, LoadFile loadDelegate )
        {
            string text = loadDelegate(fileName);

            if (text == string.Empty || text == "")
                return false;

            text = text.Trim();
            int index = 0;
            int line = 0;
            int end = 0;
            string tempstr = string.Empty;

            while (end > -1)
            {
                end = text.IndexOf("\r\n", index);
                if (end == -1)
                {
                    tempstr = text.Substring(index, text.Length - index);
                }
                else
                {
                    tempstr = text.Substring(index, end - index);
                }

                ParseStr(tempstr, line);
                line++;

                index = end + 1;
            }
            return true;
        }


        // Parse line content
        void ParseStr(string str, int line)
        {
            int index = 0;
            int end = 0;
            int dataNum = 0;

            if (line == 0)  // load data type
            {
                while (end > -1)
                {
                    end = str.IndexOf(",", index);
                    string tempStr = string.Empty;
                    if (end > -1)
                    {
                        tempStr = str.Substring(index, end - index);
                    }
                    else
                    {
                        tempStr = str.Substring(index, str.Length - index);
                    }
                    index = end + 1;

                    m_DataTypeArray.Add(tempStr);
                }
            }
            else if (line > 0)  // load table data, skip line 2, because line 2 are attributes' name and only for designer.
            {
                ArrayList mRowDataList = new ArrayList(16);  
                m_DataList.Add(mRowDataList);
                dataNum = 0;
                while (dataNum < m_DataTypeArray.Count)
                {
                    end = str.IndexOf(",", index);
                    string tempStr = string.Empty;
                    if (end > -1)
                    {
                        tempStr = str.Substring(index, end - index);
                    }
                    else
                    {
                        if (index < str.Length)
                        {
                            tempStr = str.Substring(index, str.Length - index);
                        }
                        else
                        {
                            tempStr = string.Empty;
                        }
                        end = str.Length - 1;
                    }
                    index = end + 1;

                    dataNum++;

                    mRowDataList.Add(tempStr);
                }
            }
        }


        public int getRowNum()
        {
            return m_DataList.Count;
        }


        public void SeekTowList( int col )
        {
            if (col < m_DataList.Count)
            {
                m_RowListNow = (ArrayList)m_DataList[col];
            }
        }



        public string getString(int col)
        {
            if (col >= m_DataTypeArray.Count)
                return null;

            return (string)m_RowListNow[col];
        }
    }
}
