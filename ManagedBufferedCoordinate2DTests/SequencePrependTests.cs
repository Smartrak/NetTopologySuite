﻿using System;
using GeoAPI.Coordinates;
using NetTopologySuite.Coordinates;
using NUnit.Framework;

namespace ManagedBufferedCoordinate2DTests
{
    using IBufferedCoordSequence = ICoordinateSequence<BufferedCoordinate2D>;

    [TestFixture]
    public class SequencePrependTests
    {
        private const int BigMaxLimit = Int32.MaxValue - 2;

        [Test]
        public void CoordinateToNewSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);

            BufferedCoordinate2D coord = generator.RandomCoordinate();

            generator.Sequence.Prepend(coord);

            Assert.AreEqual(coord, generator.Sequence[0]);
            Assert.AreEqual(generator.MainList[0], generator.Sequence[1]);
        }

        [Test]
        public void CoordinateToNewSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D coord = generator.RandomCoordinate();

            slice.Prepend(coord);

            Assert.AreEqual(coord, slice[0]);
            Assert.AreEqual(generator.MainList[1], slice[1]);
        }

        [Test]
        public void CoordinateToPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            generator.Sequence.Prepend(coord1);

            Assert.AreEqual(coord1, generator.Sequence[0]);
            // prepend pushes back the indexes, so the first index in MainList will 
            // be the second in Sequence
            Assert.AreEqual(generator.MainList[0], generator.Sequence[1]);

            generator.Sequence.Prepend(coord0);

            Assert.AreEqual(coord0, generator.Sequence[0]);
            Assert.AreEqual(coord1, generator.Sequence[1]);
            Assert.AreEqual(generator.MainList[0], generator.Sequence[2]);
        }

        [Test]
        public void CoordinateToPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            slice.Prepend(coord1);

            Assert.AreEqual(coord1, slice[0]);
            Assert.AreEqual(generator.MainList[1], slice[1]);

            slice.Prepend(coord0);

            Assert.AreEqual(coord0, slice[0]);
            Assert.AreEqual(coord1, slice[1]);
            Assert.AreEqual(generator.MainList[1], slice[2]);
        }

        [Test]
        public void EnumerationToNewSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);

            generator.Sequence.Prepend(prependList);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }
            Assert.AreEqual(generator.MainList[0], generator.Sequence[i]);
        }

        [Test]
        public void EnumerationToNewSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            slice.Prepend(prependList);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }
            Assert.AreEqual(generator.MainList[1], slice[i]);
        }

        [Test]
        public void EnumerationToPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate = generator.CoordinateFactory.Create(1, 1);
            generator.Sequence.Prepend(prependedCoordinate);

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            generator.Sequence.Prepend(prependList);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }
            Assert.AreEqual(prependedCoordinate, generator.Sequence[i]);
            Assert.AreEqual(generator.MainList[0], generator.Sequence[i + 1]);
        }

        [Test]
        public void EnumerationToPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate = generator.CoordinateFactory.Create(1, 1);
            slice.Prepend(prependedCoordinate);

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            slice.Prepend(prependList);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }
            Assert.AreEqual(prependedCoordinate, slice[i]);
            Assert.AreEqual(generator.MainList[1], slice[i + 1]);
        }

        [Test]
        public void SequenceToNewSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            ICoordinateSequence<BufferedCoordinate2D> prependSeq
                = generator.SequenceFactory.Create(generator.PrependList);

            generator.Sequence.Prepend(prependSeq);

            Int32 i = 0;
            BufferedCoordinate2D expected;
            BufferedCoordinate2D actual;

            for (; i < generator.PrependList.Count; i++)
            {
                expected = generator.PrependList[i];
                actual = generator.Sequence[i];
                Assert.AreEqual(expected, actual);
            }

            expected = generator.MainList[0];
            actual = generator.Sequence[i];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SequenceToNewSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            slice.Prepend(prependSeq);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }
            Assert.AreEqual(generator.MainList[1], slice[i]);
        }

        [Test]
        public void SequenceToPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);

            ICoordinateSequence<BufferedCoordinate2D> prependSeq
                = generator.SequenceFactory.Create(generator.PrependList);

            generator.Sequence.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(prependedCoordinate, generator.Sequence[i]);
            Assert.AreEqual(generator.MainList[0], generator.Sequence[i + 1]);
        }

        [Test]
        public void SequenceToPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            slice.Prepend(prependedCoordinate);

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            slice.Prepend(prependSeq);

            Int32 i = 0;
            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }
            Assert.AreEqual(prependedCoordinate, slice[i]);
            Assert.AreEqual(generator.MainList[1], slice[i + 1]);
        }

        [Test]
        public void ComplexSliceToPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);

            IBufferedCoordSequence prependSlice
                = generator.SequenceFactory.Create(generator.PrependList)
                .Slice(0, 2);
            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            generator.Sequence.Prepend(prependSlice);

            Assert.AreEqual(preSliceCoordinate, generator.Sequence[0]);
            Int32 i = 1;
            for (; i <= generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i - 1], generator.Sequence[i]);
            }
            Assert.AreEqual(postSliceCoordinate, generator.Sequence[i]);
            Assert.AreEqual(prependedCoordinate, generator.Sequence[i + 1]);
            Assert.AreEqual(generator.MainList[0], generator.Sequence[i + 2]);
        }

        [Test]
        public void ComplexSliceToPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence target = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate
                = generator.RandomCoordinate();

            target.Prepend(prependedCoordinate);

            IBufferedCoordSequence prependSlice = generator.SequenceFactory
                                                    .Create(generator.PrependList)
                                                    .Slice(0, 2);

            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            Assert.AreEqual(preSliceCoordinate, prependSlice.First);
            Assert.AreEqual(postSliceCoordinate, prependSlice.Last);

            target.Prepend(prependSlice);

            Assert.AreEqual(preSliceCoordinate, target[0]);

            Int32 i = 1;

            for (; i <= generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i - 1], target[i]);
            }

            Assert.AreEqual(postSliceCoordinate, target[i]);
            Assert.AreEqual(prependedCoordinate, target[i + 1]);
            Assert.AreEqual(generator.MainList[1], target[i + 2]);
        }

        [Test]
        public void CoordinateToNewReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            generator.Sequence.Reverse();

            BufferedCoordinate2D coord = generator.RandomCoordinate();

            generator.Sequence.Prepend(coord);

            Assert.AreEqual(coord, generator.Sequence[0]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[1]);
        }

        [Test]
        public void CoordinateToNewReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);
            slice.Reverse();

            BufferedCoordinate2D coord = generator.RandomCoordinate();

            slice.Prepend(coord);

            Assert.AreEqual(coord, slice[0]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[1]);
        }

        [Test]
        public void CoordinateToPrependedReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            generator.Sequence.Reverse();
            generator.Sequence.Prepend(coord1);

            Assert.AreEqual(coord1, generator.Sequence[0]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[1]);

            generator.Sequence.Prepend(coord0);

            Assert.AreEqual(coord0, generator.Sequence[0]);
            Assert.AreEqual(coord1, generator.Sequence[1]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[2]);
        }

        [Test]
        public void CoordinateToReversedPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            generator.Sequence.Prepend(coord1);

            BufferedCoordinate2D expected;
            BufferedCoordinate2D actual;

            expected = coord1;
            actual = generator.Sequence[0];
            Assert.AreEqual(expected, actual);

            expected = generator.MainList[0];
            actual = generator.Sequence[1];
            Assert.AreEqual(expected, actual);

            generator.Sequence.Reverse();

            generator.Sequence.Prepend(coord0);

            Assert.AreEqual(coord0, generator.Sequence[0]);

            expected = generator.MainList[generator.MainList.Count - 1];
            actual = generator.Sequence[1];
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(coord1, generator.Sequence.Last);
        }

        [Test]
        public void CoordinateToPrependedReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            slice.Reverse();
            slice.Prepend(coord1);

            Assert.AreEqual(coord1, slice[0]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[1]);

            slice.Prepend(coord0);

            Assert.AreEqual(coord0, slice[0]);
            Assert.AreEqual(coord1, slice[1]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[2]);
        }

        [Test]
        public void CoordinateToReversedPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D coord1 = generator.RandomCoordinate();
            BufferedCoordinate2D coord0 = generator.RandomCoordinate();

            slice.Prepend(coord1);

            BufferedCoordinate2D expected;
            BufferedCoordinate2D actual;

            expected = coord1;
            actual = slice[0];
            Assert.AreEqual(expected, actual);

            expected = generator.MainList[1];
            actual = slice[1];
            Assert.AreEqual(expected, actual);

            slice.Reverse();
            slice.Prepend(coord0);

            expected = coord0;
            actual = slice[0];
            Assert.AreEqual(expected, actual);

            expected = generator.MainList[3];
            actual = slice[1];
            Assert.AreEqual(expected, actual);

            expected = coord1;
            actual = slice.Last;
            Assert.AreEqual(coord1, slice.Last);
        }

        [Test]
        public void EnumerationToNewReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            generator.Sequence.Reverse();

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            generator.Sequence.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i]);
        }

        [Test]
        public void EnumerationToNewReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);
            slice.Reverse();

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            slice.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i]);
        }

        [Test]
        public void EnumerationToPrependedReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            generator.Sequence.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.CoordinateFactory.Create(1, 1);
            generator.Sequence.Prepend(prependedCoordinate);

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            generator.Sequence.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(prependedCoordinate, generator.Sequence[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i + 1]);
        }

        [Test]
        public void EnumerationToReversedPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate
                = generator.CoordinateFactory.Create(1, 1);

            generator.Sequence.Prepend(prependedCoordinate);
            generator.Sequence.Reverse();

            Buffered2DEnumeratorHelper prependList
                = new Buffered2DEnumeratorHelper(generator.PrependList);
            generator.Sequence.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            BufferedCoordinate2D expected;
            BufferedCoordinate2D actual;

            // last coordinate in MainList (index 4) is the same as the 
            // first coordinate of the sequence after the prepended coordinates, 
            // at index 3
            expected = generator.MainList[4];
            actual = generator.Sequence[3];
            Assert.AreEqual(expected, actual);

            expected = prependedCoordinate;
            actual = generator.Sequence.Last;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EnumerationToPrependedReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);
            slice.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.CoordinateFactory.Create(1, 1);
            slice.Prepend(prependedCoordinate);

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            slice.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(prependedCoordinate, slice[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i + 1]);
        }

        [Test]
        public void EnumerationToReversedPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate = generator.CoordinateFactory.Create(1, 1);
            slice.Prepend(prependedCoordinate);
            slice.Reverse();

            Buffered2DEnumeratorHelper prependList = new Buffered2DEnumeratorHelper(generator.PrependList);
            slice.Prepend(prependList);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i]);
            Assert.AreEqual(prependedCoordinate, slice.Last);
        }

        [Test]
        public void SequenceToNewReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            generator.Sequence.Reverse();
            generator.Sequence.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i]);
        }

        [Test]
        public void SequenceToNewReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);
            slice.Reverse();

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            slice.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i]);
        }

        [Test]
        public void SequenceToPrependedReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            generator.Sequence.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);

            ICoordinateSequence<BufferedCoordinate2D> prependSeq
                = generator.SequenceFactory.Create(generator.PrependList);

            generator.Sequence.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(prependedCoordinate, generator.Sequence[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i + 1]);
        }

        [Test]
        public void SequenceToReversedPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);
            generator.Sequence.Reverse();

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            generator.Sequence.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], generator.Sequence[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i]);
            Assert.AreEqual(prependedCoordinate, generator.Sequence.Last);
        }

        [Test]
        public void SequenceToPrependedReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);
            slice.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            slice.Prepend(prependedCoordinate);

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            slice.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(prependedCoordinate, slice[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i + 1]);
        }

        [Test]
        public void SequenceToReversedPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            slice.Prepend(prependedCoordinate);
            slice.Reverse();

            ICoordinateSequence<BufferedCoordinate2D> prependSeq = generator.SequenceFactory.Create(generator.PrependList);

            slice.Prepend(prependSeq);

            Int32 i = 0;

            for (; i < generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i], slice[i]);
            }

            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i]);
            Assert.AreEqual(prependedCoordinate, slice.Last);
        }

        [Test]
        public void ComplexSliceToPrependedReversedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            generator.Sequence.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);

            IBufferedCoordSequence prependSlice
                = generator.SequenceFactory.Create(generator.PrependList)
                .Slice(0, 2);
            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            generator.Sequence.Prepend(prependSlice);

            Assert.AreEqual(preSliceCoordinate, generator.Sequence[0]);

            Int32 i = 1;

            for (; i <= generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i - 1], generator.Sequence[i]);
            }

            Assert.AreEqual(postSliceCoordinate, generator.Sequence[i]);
            Assert.AreEqual(prependedCoordinate, generator.Sequence[i + 1]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i + 2]);
        }

        [Test]
        public void ComplexSliceToReversedPrependedSequence()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            generator.Sequence.Prepend(prependedCoordinate);
            generator.Sequence.Reverse();

            IBufferedCoordSequence prependSlice
                = generator.SequenceFactory.Create(generator.PrependList)
                .Slice(0, 2);
            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            generator.Sequence.Prepend(prependSlice);

            Assert.AreEqual(preSliceCoordinate, generator.Sequence[0]);

            Int32 i = 1;

            for (; i <= generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i - 1], generator.Sequence[i]);
            }

            Assert.AreEqual(postSliceCoordinate, generator.Sequence[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 1], generator.Sequence[i + 1]);
            Assert.AreEqual(prependedCoordinate, generator.Sequence.Last);
        }

        [Test]
        public void ComplexSliceToPrependedReversedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence target = generator.Sequence.Slice(1, 3);
            target.Reverse();

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            target.Prepend(prependedCoordinate);

            IBufferedCoordSequence prependSlice
                = generator.SequenceFactory.Create(generator.PrependList)
                  .Slice(0, 2);

            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            target.Prepend(prependSlice);

            BufferedCoordinate2D expected;
            BufferedCoordinate2D actual;

            expected = preSliceCoordinate;
            actual = target[0];
            Assert.AreEqual(expected, actual);

            Int32 i = 1;

            for (; i <= generator.PrependList.Count; i++)
            {
                expected = generator.PrependList[i - 1];
                actual = target[i];
                Assert.AreEqual(expected, actual);
            }

            expected = postSliceCoordinate;
            actual = target[i];
            Assert.AreEqual(expected, actual);

            expected = prependedCoordinate;
            actual = target[i + 1];
            Assert.AreEqual(expected, actual);

            expected = generator.MainList[generator.MainList.Count - 2];
            actual = target[i + 2];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ComplexSliceToReversedPrependedSlice()
        {
            SequenceGenerator generator = new SequenceGenerator(BigMaxLimit, 5, 3, 0);
            IBufferedCoordSequence slice = generator.Sequence.Slice(1, 3);

            BufferedCoordinate2D prependedCoordinate = generator.RandomCoordinate();
            slice.Prepend(prependedCoordinate);
            slice.Reverse();

            IBufferedCoordSequence prependSlice
                = generator.SequenceFactory.Create(generator.PrependList)
                  .Slice(0, 2);
            BufferedCoordinate2D preSliceCoordinate = generator.RandomCoordinate();
            BufferedCoordinate2D postSliceCoordinate = generator.RandomCoordinate();
            prependSlice.Prepend(preSliceCoordinate);
            prependSlice.Append(postSliceCoordinate);

            slice.Prepend(prependSlice);

            Assert.AreEqual(preSliceCoordinate, slice[0]);

            Int32 i = 1;

            for (; i <= generator.PrependList.Count; i++)
            {
                Assert.AreEqual(generator.PrependList[i - 1], slice[i]);
            }

            Assert.AreEqual(postSliceCoordinate, slice[i]);
            Assert.AreEqual(generator.MainList[generator.MainList.Count - 2], slice[i + 1]);
            Assert.AreEqual(prependedCoordinate, slice.Last);
        }

    }
}