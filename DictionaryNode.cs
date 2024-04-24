namespace Dictionary;

class DictionaryNode
    {
        public readonly Dictionary<string, List<string>> term;
        public byte height;
        public DictionaryNode left, right;
        public DictionaryNode(Dictionary<string, List<string>> k)
        {
            term = k;
            left = right = null;
            height = 1;
        }

        public DictionaryNode()
        {
            term = null;
            left = right = null;
            height = 1;
        }
    }

    class BinaryDictionaryTree
    {
        public DictionaryNode Insert(DictionaryNode p, Dictionary<string, List<string>> k)
        {
            if (p == null)
                return new DictionaryNode(k);

            // Сравнение по алфавиту ключей
            int comparisonResult = CompareKeys(k, p.term);

            if (comparisonResult < 0)
                p.left = Insert(p.left, k);
            else
                p.right = Insert(p.right, k);

            return Balance(p);
        }

        public DictionaryNode Remove(DictionaryNode root, Dictionary<string, List<string>> k)
        {
            if (root == null)
                return null;

            int comparisonResult = CompareKeys(k, root.term);

            if (comparisonResult < 0)
                root.left = Remove(root.left, k);
            else if (comparisonResult > 0)
                root.right = Remove(root.right, k);
            else // k == root.Key
            {
                if (root.right == null)
                    return root.left;

                DictionaryNode q = root.left;
                DictionaryNode r = root.right;
                root = null; // Удаление ссылки на текущий узел

                DictionaryNode min = FindMin(r);
                min.right = RemoveMin(r);
                min.left = q;
                return Balance(min);
            }
            return Balance(root);
        }

        public void PrintTree(DictionaryNode root)
        {
            if (root != null)
            {
                PrintTree(root.left);
                Console.WriteLine(
                    $"{root.term.Keys.First()} - {string.Join("; ", root.term.Values.SelectMany(x => x))}");
                PrintTree(root.right);
            }
        }

        public DictionaryNode Search(DictionaryNode root, string term)
        {
            if (root == null || root.term.Keys.FirstOrDefault()?.ToLower() == term.ToLower())
                return root;

            return Search(String.CompareOrdinal(term.ToLower(), root.term.Keys.FirstOrDefault()?.ToLower()) < 0 ? root.left : root.right, term);
        }
        
        private byte Height(DictionaryNode p) => p != null ? p.height : (byte)0;

        private int BalanceFactor(DictionaryNode p) => Height(p.right) - Height(p.left);

        private void FixHeight(DictionaryNode p)
        {
            byte leftHeight = Height(p.left);
            byte rightHeight = Height(p.right);
            p.height = (byte)(leftHeight > rightHeight ? leftHeight : rightHeight + 1);
        }

        private DictionaryNode RotateRight(DictionaryNode p)
        {
            DictionaryNode q = p.left;
            p.left = q.right;
            q.right = p;
            FixHeight(p);
            FixHeight(q);
            return q;
        }

        private DictionaryNode RotateLeft(DictionaryNode q)
        {
            DictionaryNode p = q.right;
            q.right = p.left;
            p.left = q;
            FixHeight(q);
            FixHeight(p);
            return p;
        }

        private DictionaryNode Balance(DictionaryNode p)
        {
            FixHeight(p);
            if (BalanceFactor(p) == 2)
            {
                if (BalanceFactor(p.right) < 0)
                    p.right = RotateRight(p.right);
                return RotateLeft(p);
            }
            if (BalanceFactor(p) == -2)
            {
                if (BalanceFactor(p.left) > 0)
                    p.left = RotateLeft(p.left);
                return RotateRight(p);
            }
            return p; // Балансировка не нужна
        }

        // Функция для сравнения ключей
        private int CompareKeys(Dictionary<string, List<string>> key1, Dictionary<string, List<string>> key2)
        {
            // В данном случае будем сравнивать первые слова в ключах по алфавиту
            var term1 = key1.Keys.FirstOrDefault();
            var term2 = key2.Keys.FirstOrDefault();

            return term1.CompareTo(term2);
        }

        private DictionaryNode FindMin(DictionaryNode p) => p.left != null ? FindMin(p.left) : p;

        private DictionaryNode RemoveMin(DictionaryNode p)
        {
            if (p.left == null)
                return p.right;

            p.left = RemoveMin(p.left);
            return Balance(p);
        }
    }