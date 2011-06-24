using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace Scratchpad
{
    /// <summary>
    /// Provides a fluent interface for asserting the raising of exceptions.
    /// </summary>
    /// <typeparam name="TException">The type of exception to assert.</typeparam>
    public static class Exception<TException> 
        where TException : Exception
    {
        /// <summary>
        /// Asserts that the exception is thrown when the specified action is executed.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        public static TException ShouldBeThrownBy(Action action)
        {
            TException exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                exception = e.ShouldBeOfType<TException>();
            }

            exception.ShouldNotBeNull("An exception was expected, but not thrown by the given action.");

            return exception;
        }

        public static TException ShouldBeThrownBy(Action action, Action<TException> continuation)
        {
            TException exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                exception = e.ShouldBeOfType<TException>();
                continuation((TException)e);
            }

            exception.ShouldNotBeNull("An exception was expected, but not thrown by the given action.");

            return exception;
        }

        /// <summary>
        /// Asserts that the exception was thrown when the specified action is executed by
        /// scanning the exception chain.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        public static TException ShouldExistInExceptionChainAfter(Action action)
        {
            TException exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                exception = ScanExceptionChain(e);
            }

            return exception;
        }
        /// <summary>
        /// Scans the specified exception chain and asserts the existence of the target exception.
        /// </summary>
        /// <param name="chainLink">Exception chain to scan.</param>
        /// <returns></returns>
        private static TException ScanExceptionChain(Exception chainLink)
        {
            TException exception;
            if((exception = chainLink as TException) != null)
            {
                return exception;
            }

            if(chainLink.InnerException == null)
            {
                Assert.Fail("An exception was expected, but not thrown by the given action.");
            }

            return ScanExceptionChain(chainLink.InnerException);
        }
    }

    public static class SpecificationExtensions
    {
        public static T ShouldTransferViaSerialization<T>(this T instance)
        {
            var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, instance);
            stream.Position = 0;

            return new BinaryFormatter().Deserialize(stream).ShouldBeOfType<T>();
        }

        public static void ShouldHave<T>(this IEnumerable<T> values, Func<T, bool> func)
        {
            values.FirstOrDefault(func).ShouldNotBeNull();
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static void ShouldBeTrueBecause(this bool condition, string reason, params object[] args)
        {
            Assert.IsTrue(condition, reason, args);
        }

        public static object ShouldEqual(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static object ShouldEqual(this string actual, object expected)
        {
            Assert.AreEqual((expected != null) ? expected.ToString() : null, actual);
            return expected;
        }

        public static void ShouldMatch(this string actual, string pattern)
        {
#pragma warning disable 612,618
            Assert.That(actual, Text.Matches(pattern));
#pragma warning restore 612,618
        }

        public static XmlElement AttributeShouldEqual(this XmlElement element, string attributeName, object expected)
        {
            Assert.IsNotNull(element, "The Element is null");

            string actual = element.GetAttribute(attributeName);
            Assert.AreEqual(expected, actual);
            return element;
        }

        public static XmlElement AttributeShouldEqual(this XmlNode node, string attributeName, object expected)
        {
            var element = node as XmlElement;

            Assert.IsNotNull(element, "The Element is null");

            string actual = element.GetAttribute(attributeName);
            Assert.AreEqual(expected, actual);
            return element;
        }

        public static XmlElement ShouldHaveChild(this XmlElement element, string xpath)
        {
            var child = element.SelectSingleNode(xpath) as XmlElement;
            Assert.IsNotNull(child, "Should have a child element matching " + xpath);

            return child;
        }

        public static XmlElement DoesNotHaveAttribute(this XmlElement element, string attributeName)
        {
            Assert.IsNotNull(element, "The Element is null");
            Assert.IsFalse(element.HasAttribute(attributeName),
                           "Element should not have an attribute named " + attributeName);

            return element;
        }

        public static object ShouldNotEqual(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static void ShouldNotBeNull(this object anObject)
        {
            Assert.IsNotNull(anObject);
        }

        public static void ShouldNotBeNull(this object anObject, string message)
        {
            Assert.IsNotNull(anObject, message);
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        public static T ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType(typeof(T));
            return (T)actual;
        }

        public static object ShouldBeOfType(this object actual, Type expected)
        {
#pragma warning disable 612,618
            Assert.IsInstanceOfType(expected, actual);
#pragma warning restore 612,618
            return actual;
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
#pragma warning disable 612,618
            Assert.IsNotInstanceOfType(expected, actual);
#pragma warning restore 612,618
        }

        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.Contains(expected, actual);
        }

        public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> actual, T expected)
        {
            if (actual.Count(t => t.Equals(expected)) == 0)
            {
                Assert.Fail("The item '{0}' was not found in the sequence.", expected);
            }

            return actual;
        }

        public static void ShouldNotBeEmpty<T>(this IEnumerable<T> actual)
        {
            Assert.Greater(actual.Count(), 0, "The list should have at least one element");
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, T expected)
        {
            if (actual.Count(t => t.Equals(expected)) > 0)
            {
                Assert.Fail("The item was found in the sequence it should not be in.");
            }
        }

        public static void ShouldHaveTheSameElementsAs(this IList actual, IList expected)
        {
            actual.ShouldNotBeNull();
            expected.ShouldNotBeNull();

            actual.Count.ShouldEqual(expected.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                actual[i].ShouldEqual(expected[i]);
            }
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, params T[] expected)
        {
            ShouldHaveTheSameElementsAs(actual, (IEnumerable<T>)expected);
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IList actualList = (actual is IList) ? (IList)actual : actual.ToList();
            IList expectedList = (expected is IList) ? (IList)expected : expected.ToList();

            ShouldHaveTheSameElementsAs(actualList, expectedList);
        }

        public static void ShouldHaveTheSameElementKeysAs<ELEMENT, KEY>(this IEnumerable<ELEMENT> actual,
                                                                        IEnumerable expected,
                                                                        Func<ELEMENT, KEY> keySelector)
        {
            actual.ShouldNotBeNull();
            expected.ShouldNotBeNull();

            ELEMENT[] actualArray = actual.ToArray();
            object[] expectedArray = expected.Cast<object>().ToArray();

            actualArray.Length.ShouldEqual(expectedArray.Length);

            for (int i = 0; i < actual.Count(); i++)
            {
                keySelector(actualArray[i]).ShouldEqual(expectedArray[i]);
            }
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
            return arg2;
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
            return arg2;
        }

        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsNotEmpty(collection);
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }

        public static void ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }


        public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
        {
            actual.Count().ShouldBeGreaterThan(0);
            T result = actual.FirstOrDefault(expected);
            Assert.That(result, Is.Not.EqualTo(default(T)), "Expected item was not found in the actual sequence");
        }

        public static void ShouldNotContain(this string actual, string expected)
        {
            Assert.That(actual, new NotConstraint(new SubstringConstraint(expected)));
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return expected;
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
        }

        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, Action action)
        {
            Exception exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exceptionType, e.GetType());
                exception = e;
            }

            if (exception == null)
            {
                Assert.Fail(String.Format("Expected {0} to be thrown.", exceptionType.FullName));
            }

            return exception;
        }


        public static void ShouldEqualSqlDate(this DateTime actual, DateTime expected)
        {
            TimeSpan timeSpan = actual - expected;
            Assert.Less(Math.Abs(timeSpan.TotalMilliseconds), 3);
        }


        public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> actual, int expected)
        {
            actual.Count().ShouldEqual(expected);
            return actual;
        }

    }
}