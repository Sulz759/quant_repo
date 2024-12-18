﻿using System;
using System.Collections.Generic;

namespace _Project.Develop.Architecture.Runtime.Utilities
{
    public class ShuffleBag<T>
    {
        private readonly Dictionary<T, int> cooldowns = new();
        private int currentIteration;
        private readonly Dictionary<T, int> itemCounts = new();
        private readonly List<T> items = new();
        private readonly Dictionary<T, int> lastIteration = new();
        private readonly Random random = new();

        public void Add(T item, int count, int cooldown)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item] += count;
            }
            else
            {
                itemCounts.Add(item, count);
                cooldowns.Add(item, cooldown);
                lastIteration.Add(item, -cooldown);
            }

            for (var i = 0; i < count; i++) items.Add(item);
        }

        public T GetNext()
        {
            if (currentIteration < 5)
            {
                currentIteration++;
                return items[0]; // подумать над словариком, непонятно, что делать при смене нодов
            }

            if (currentIteration == 19)
            {
                currentIteration++;
                return items[1];
            }

            if (items.Count == 0) Refill();

            T item;
            do
            {
                var index = random.Next(items.Count);
                item = items[index];
                //items.RemoveAt(index);
            } while (currentIteration - lastIteration[item] < cooldowns[item]);

            lastIteration[item] = currentIteration;
            currentIteration++;

            return item;
        }

        public void Refill()
        {
            items.Clear();

            foreach (var pair in itemCounts)
                for (var i = 0; i < pair.Value; i++)
                    items.Add(pair.Key);
        }

        public void Reset()
        {
            items.Clear();
            itemCounts.Clear();
            cooldowns.Clear();
            lastIteration.Clear();
            currentIteration = 0;
        }

        public void SetWeight(T item, int newCount, int newCooldown)
        {
            if (itemCounts.ContainsKey(item))
            {
                var oldCount = itemCounts[item];
                itemCounts[item] = newCount;
                cooldowns[item] = newCooldown;

                // Обновляем количество элементов в items
                var countDiff = newCount - oldCount;
                if (countDiff > 0)
                    for (var i = 0; i < countDiff; i++)
                        items.Add(item);
                else if (countDiff < 0)
                    for (var i = 0; i < -countDiff; i++)
                        items.Remove(item);
            }
            else
            {
                Add(item, newCount, newCooldown);
            }
        }
    }
}