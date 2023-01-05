using System;
using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO;

static class EnumerableEx
{
    public static IEnumerable<T> StartWith<T>(this IEnumerable<T> self, params T[] items)
    {
        return items.Concat(self);
    }

    public static IEnumerable<T[]> Buffer<T>(this IEnumerable<T> self, int size)
    {
        var list = new List<T>();

        foreach (var i in self)
        {
            list.Add(i);

            if (list.Count >= size)
            {
                yield return list.ToArray();

                list.Clear();
            }
        }

        if (list.Any())
            yield return list.ToArray();
    }

    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        foreach (var i in self)
            action(i);
    }

    public static void ForEach<T>(this IEnumerable<T> self, Action<T, int> action)
    {
        var idx = 0;

        foreach (var i in self)
            action(i, idx++);
    }

    public static IEnumerable<T> Repeat<T>(T element)
    {
        while (true)
            yield return element;
    }

    public static IEnumerable<T> Defer<T>(Func<IEnumerable<T>> func)
    {
        return func();
    }
}