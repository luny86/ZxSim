using System.Collections;
using System.Collections.Generic;
using KUtil;

namespace ThreeWeeks
{
    internal class FurnitureEnumerator : IEnumerator<CodeArgs>
    {
        #region Fields
        private int _index;
        private CodeArgs _current;
        #endregion

        #region Construction
        public FurnitureEnumerator(int startOffset, 
            IChunk furnitureData,
            IReadOnlyDictionary<byte, CodeInfo> info,
            IReadOnlyList<Range> ranges)
        {
            StartOffset = startOffset;
            FurnitureData = furnitureData;
            CodeInfo = info;
            Ranges = ranges;
            _current = null;
            (this as IEnumerator).Reset();
        }

        public void Dispose()
        {
            _current = null;
        }
        #endregion

        #region Properties
        private int StartOffset { get; }
        private IChunk FurnitureData { get; }
        private IReadOnlyDictionary<byte, CodeInfo> CodeInfo { get; }
        private IReadOnlyList<Range> Ranges { get; }

        public CodeArgs Current => _current;

        object IEnumerator.Current => _current;
        #endregion

        #region IEnumerator
        bool IEnumerator.MoveNext()
        {
            _current = null;

            byte code = CurrentCode();

            bool valid = code != (byte)FurnitureCode.Exit;

            if(valid)
            {
                _current = CreateArgs(code);
                _index += _current.Info.NumberOfArgs;
            }

            return valid;
        }

        void IEnumerator.Reset()
        {
            _index = StartOffset;
            _current = CreateArgs(CurrentCode());
        }
        #endregion

        #region Private Helpers
        private byte CurrentCode()
        {
            return CheckForRangeCode(FurnitureData[_index]);
        }

        private CodeArgs CreateArgs(byte code)
        {
            CodeInfo info = CodeInfo[code];

            return new CodeArgs(
                FurnitureData.CopyRange(_index, info.NumberOfArgs),
                info);
        }
 
         private byte CheckForRangeCode(byte code)
        {
            System.Diagnostics.Debug.Assert(Ranges != null, "Ranges should contain stuff");
            foreach(Range range in Ranges)
            {
                if(range.Within(code))
                {
                    code = range.LessThan;
                    break;
                }
            }

            return code;
        }
       #endregion
    }
}