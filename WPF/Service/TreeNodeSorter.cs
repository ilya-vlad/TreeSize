using Aga.Controls.Tree;
using System;
using System.Collections;
using System.ComponentModel;
using WPF.Common;

namespace WPF.Service
{
    public class TreeNodeSorter : IComparer
    {
        private ColumnSort _sortBy;

        private ListSortDirection _direction;

        public TreeNodeSorter(ColumnSort sortBy, ListSortDirection direction)
        {
            _sortBy = sortBy;
            _direction = direction;
        }

        public int Compare(object x, object y)
        {
            var firstTreeNode = (TreeNode) x;
            var secondTreeNode = (TreeNode) y;

            var firstNode = firstTreeNode.Tag as Node;
            var secondNode = secondTreeNode.Tag as Node;

            if (firstNode.Children.Contains(secondNode))
            {         
                return -1;
            }

            if (secondNode.Children.Contains(firstNode))
            {
                return 1;
            }

            if (firstTreeNode.Level != secondTreeNode.Level)
            {
                if (firstTreeNode.Level < secondTreeNode.Level)
                {
                    return Compare(firstTreeNode, secondTreeNode.Parent);
                }
                else
                {
                    return Compare(firstTreeNode.Parent, secondTreeNode);
                }
            }
            else
            {
                if (firstNode.NodeParent.Id != secondNode.NodeParent.Id)
                {
                    return Compare(firstTreeNode.Parent, secondTreeNode.Parent);
                }
            }

            int result = _sortBy switch
            {
                ColumnSort.Name => string.Compare(firstNode.Name, secondNode.Name),
                ColumnSort.Size => firstNode.Size.CompareTo(secondNode.Size),
                ColumnSort.Allocated => firstNode.Allocated.CompareTo(secondNode.Allocated),
                ColumnSort.CountFolders => firstNode.CountFolders.CompareTo(secondNode.CountFolders),
                ColumnSort.CountFiles => firstNode.CountFiles.CompareTo(secondNode.CountFiles),
                ColumnSort.PercentOfParent => firstNode.PercentOfParent.CompareTo(secondNode.PercentOfParent),
                ColumnSort.LastModified => DateTime.Compare(firstNode.LastModified, secondNode.LastModified),
                _ => throw new ArgumentNullException(nameof(_sortBy))
            };

            if(_direction == ListSortDirection.Descending)
            {
                result *= (-1);
            }

            return result;
        }
    }
}
