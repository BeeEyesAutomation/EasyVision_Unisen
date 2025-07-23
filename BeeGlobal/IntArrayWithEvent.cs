using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
  
        public class IntArrayWithEvent
        {
            public  readonly int[] _inner;
            public event EventHandler<ElementChangedEventArgs> ItemChanged;
            public event EventHandler<BulkChangedEventArgs> BulkChanged;

            public IntArrayWithEvent(int length)
                => _inner = new int[length];

            public int this[int index]
            {
                get => _inner[index];
                set
                {
                    if (_inner[index] == value) return;
                    int old = _inner[index];
                    _inner[index] = value;
                    ItemChanged?.Invoke(this, new ElementChangedEventArgs(index, old, value));
                }
            }

            public int Length => _inner.Length;

            /// <summary>
            /// Thay toàn bộ mảng bằng newValues (cùng độ dài hoặc lớn hơn).
            /// Gọi BulkChanged chỉ một lần.
            /// </summary>
            public void ReplaceAll(int[] newValues)
            {
                if (newValues == null) throw new ArgumentNullException(nameof(newValues));
                if (newValues.Length < _inner.Length)
                    throw new ArgumentException("newValues phải có độ dài ≥ Length");

                // Sao lưu old values
                var oldValues = new int[_inner.Length];
                Array.Copy(_inner, oldValues, _inner.Length);

                // Ghi đè
                Array.Copy(newValues, _inner, _inner.Length);

                // Phát event bulk chỉ một lần
                BulkChanged?.Invoke(this, new BulkChangedEventArgs(oldValues, newValues, _inner));
            }

            /// <summary>
            /// Thay thế một dải liên tiếp bắt đầu từ startIndex, count phần tử.
            /// </summary>
            public void ReplaceRange(int startIndex, int[] newValues)
            {
                if (newValues == null) throw new ArgumentNullException(nameof(newValues));
                if (startIndex < 0 || startIndex + newValues.Length > _inner.Length)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));

                var oldSegment = new int[newValues.Length];
                Array.Copy(_inner, startIndex, oldSegment, 0, newValues.Length);

                Array.Copy(newValues, 0, _inner, startIndex, newValues.Length);

                BulkChanged?.Invoke(this,
                    new BulkChangedEventArgs(
                        oldSegment,
                        newValues,
                        startIndex,
                        newValues.Length
                    )
                );
            }
        }

        public class ElementChangedEventArgs : EventArgs
        {
            public int Index { get; }
            public int OldValue { get; }
            public int NewValue { get; }

            public ElementChangedEventArgs(int idx, int old, int @new)
            {
                Index = idx;
                OldValue = old;
                NewValue = @new;
            }
        }

        public class BulkChangedEventArgs : EventArgs
        {
            public int[] OldValues { get; }
            public int[] NewValues { get; }
            public int StartIndex { get; }
            public int Count { get; }

            // Toàn bộ mảng
            public BulkChangedEventArgs(int[] oldValues, int[] newValues, int[] current)
            {
                OldValues = oldValues;
                NewValues = newValues;
                StartIndex = 0;
                Count = oldValues.Length;
            }

            // Thay range
            public BulkChangedEventArgs(int[] oldSegment, int[] newSegment, int startIndex, int count)
            {
                OldValues = oldSegment;
                NewValues = newSegment;
                StartIndex = startIndex;
                Count = count;
            }
        }
    }

