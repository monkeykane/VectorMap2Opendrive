using System;
using System.Collections.Generic;
using System.Text;


namespace VectorMap2Opendrive
{
    // Base Data
    public abstract class BaseData
    {
        public int m_ID;    // the first line is always ID.
        public abstract void LoadData(int nRowIndex, DataFile file);
    }


    // Data Manager
    public abstract class BaseDataManager
    {
        protected   Dictionary<int, BaseData>       m_DataMap;
        protected   int                             m_RowNum;

        public BaseDataManager()
        {
            m_DataMap = new Dictionary<int, BaseData>(64);
        }


        public void LoadData( string fileName, DataFile.LoadFile loadDelegate )
        {
            DataFile datafile = new DataFile();
            if ( datafile.LoadData(fileName, loadDelegate ) == false )
            {               
                StringBuilder sb = new StringBuilder(256);
                sb.AppendFormat("Loading File Error: {0}", fileName);
                throw new System.Exception(sb.ToString());
            }

            m_RowNum = datafile.getRowNum();

            for (int i = 0; i < m_RowNum; i++)
            {
                BaseData item = NewItem();
                datafile.SeekTowList(i);
                try
                {
                    item.LoadData(i, datafile);
                    m_DataMap[item.m_ID] = item;
                }
                catch (System.Exception)
                {
                    StringBuilder sb = new StringBuilder(256);
                    sb.AppendFormat("Load {0} Error: row={1}", fileName, i);
                    throw new System.Exception(sb.ToString());
                }
            }

            _OnLoadComplete();

        }

        protected virtual void _OnLoadComplete()
        {
        }

        protected abstract BaseData NewItem();

        public virtual void DoDestroy()
        {

        }
    }
}
