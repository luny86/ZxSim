
using Bindings;

namespace ZX.Tests.Bindings
{
    public class BindingsManagerTests
    {
        [Test]
        public void CreateObjectTest()
        {
            BindingsManager manager = new BindingsManager();

            IBoundObject<int> reference = manager.CreateObject<int>("binding", 11);

            Assert.NotNull(reference, "Unable to create 'binding'");

            reference = manager.GetObject<int>("binding");

            Assert.NotNull(reference, "Unable to find 'binding'");
            Assert.That(reference.Value, Is.EqualTo(11));
        }

        [Test]
        public void CreateObjectTwiceTest()
        {
            BindingsManager manager = new BindingsManager();

            IBoundObject<int> one = manager.CreateObject<int>("binding", 11);
            IBoundObject<int> two = null!;
            
            Assert.DoesNotThrow(
                () => two = manager.CreateObject<int>("binding", 32));

            Assert.That(two, Is.Not.EqualTo(null), "Two should not be null.");
            Assert.That(one, Is.EqualTo(two), "Two is not the same object as one.");
            Assert.That(two.Value, Is.EqualTo(32), "Second create did not change the value.");
        }

        [Test]
        public void GetObjectNotExistTest()
        {
            BindingsManager manager = new BindingsManager();

            Assert.Throws<InvalidOperationException>(
                () => manager.GetObject<int>("binding"));
        }

        [Test]
        public void GetObjectWithWrongTypeTest()
        {
            BindingsManager manager = new BindingsManager();
            IBoundObject<string> reference = manager.CreateObject<string>("binding", "abc");

            Assert.Throws<InvalidCastException>(
                () => manager.GetObject<int>("binding"));
        }

        [Test]
        public void OnValueChangedTest()
        {
            bool eventCaught = false;

            BindingsManager manager = new BindingsManager();

            manager.CreateObject<int>("binding", 11);
            manager.Bind("binding", (name, type, value) =>
            {
                if(name == "binding" &&
                    type == typeof(int) &&
                    value is not null &&
                    (int)value  == 12)
                    {
                        eventCaught = true;
                    }
            });

            IBoundObject<int> reference = manager.GetObject<int>("binding");
            reference.Value = 12;

            Assert.That(eventCaught, Is.True);
        }
    }
}