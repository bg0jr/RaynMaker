using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec
{
    [TestFixture]
    public class DataSourceTests
    {
        [Test]
        public void Vendor_Set_ValueIsSet()
        {
            var dataSource = new DataSource();

            dataSource.Vendor = "Vendor";

            Assert.That(dataSource.Vendor, Is.EqualTo("Vendor"));
        }

        [Test]
        public void Vendor_Set_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new PropertyChangedCounter(dataSource);

            dataSource.Vendor = "Vendor";

            Assert.That(counter.GetCount(nameof(dataSource.Vendor)), Is.EqualTo(1));
        }

        [Test]
        public void Name_Set_ValueIsSet()
        {
            var dataSource = new DataSource();

            dataSource.Name = "Name";

            Assert.That(dataSource.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Name_Set_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new PropertyChangedCounter(dataSource);

            dataSource.Name = "Name";

            Assert.That(counter.GetCount(nameof(dataSource.Name)), Is.EqualTo(1));
        }

        [Test]
        public void Quality_Set_ValueIsSet()
        {
            var dataSource = new DataSource();

            dataSource.Quality = 6;

            Assert.That(dataSource.Quality, Is.EqualTo(6));
        }

        [Test]
        public void Quality_Set_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new PropertyChangedCounter(dataSource);

            dataSource.Quality = 6;

            Assert.That(counter.GetCount(nameof(dataSource.Quality)), Is.EqualTo(1));
        }

        [Test]
        public void DocumentType_Set_ValueIsSet()
        {
            var dataSource = new DataSource();

            dataSource.DocumentType = DocumentType.Html;

            Assert.That(dataSource.DocumentType, Is.EqualTo(DocumentType.Html));
        }

        [Test]
        public void DocumentType_Set_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new PropertyChangedCounter(dataSource);

            dataSource.DocumentType = DocumentType.Html;

            Assert.That(counter.GetCount(nameof(dataSource.DocumentType)), Is.EqualTo(1));
        }

        [Test]
        public void Location_Set_ValueIsSet()
        {
            var dataSource = new DataSource();

            dataSource.Location = new DocumentLocator(
                new Request("http://test1.org"),
                new Response("http://test2.org"));

            Assert.That(dataSource.Location.Fragments[0].UrlString, Is.EqualTo("http://test1.org"));
        }

        [Test]
        public void Location_Set_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new PropertyChangedCounter(dataSource);

            dataSource.Location = new DocumentLocator(
                new Request("http://test1.org"),
                new Response("http://test2.org"));

            Assert.That(counter.GetCount(nameof(dataSource.Location)), Is.EqualTo(1));
        }

        [Test]
        public void Figures_Add_ValueAdded()
        {
            var dataSource = new DataSource();

            dataSource.Figures.Add(new CsvDescriptor() { Figure = "t45" });

            Assert.That(dataSource.Figures[0].Figure, Is.EqualTo("t45"));
        }

        [Test]
        public void Figures_Add_ChangeIsNotified()
        {
            var dataSource = new DataSource();
            var counter = new CollectionChangedCounter((INotifyCollectionChanged)dataSource.Figures);

            dataSource.Figures.Add(new CsvDescriptor());

            Assert.That(counter.Count, Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.Quality = 17;
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = new DocumentLocator(
                new Request("http://test1.org"),
                new Response("http://test2.org"));
            dataSource.Figures.Add(new CsvDescriptor { Figure = "dummy.csv" });

            var clone = FigureDescriptorFactory.Clone(dataSource);

            Assert.That(clone.Vendor, Is.EqualTo("vendor"));
            Assert.That(clone.Name, Is.EqualTo("name"));
            Assert.That(clone.Quality, Is.EqualTo(17));
            Assert.That(clone.DocumentType, Is.EqualTo(DocumentType.Html));

            Assert.That(clone.Location.Fragments[0].UrlString, Is.EqualTo("http://test1.org"));

            Assert.That(clone.Figures[0].Figure, Is.EqualTo("dummy.csv"));
        }

        [Test]
        public void Clone_WhenCalled_FiguresCollectionIsMutableAndObservable()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));
            dataSource.Figures.Add(new CsvDescriptor { Figure = "dummy.csv" });

            var clone = FigureDescriptorFactory.Clone(dataSource);

            var counter = new CollectionChangedCounter((INotifyCollectionChanged)clone.Figures);

            clone.Figures.Add(new CsvDescriptor { Figure = "dummy.series" });

            Assert.That(clone.Figures[0].Figure, Is.EqualTo("dummy.csv"));
            Assert.That(clone.Figures[1].Figure, Is.EqualTo("dummy.series"));
            Assert.That(counter.Count, Is.EqualTo(1));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));

            var descriptor = new CsvDescriptor();
            descriptor.Figure = "dummy.csv";
            descriptor.Separator = ";";
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double), "0.00"));
            dataSource.Figures.Add(descriptor);

            RecursiveValidator.Validate(dataSource);
        }

        [Test]
        public void Validate_InvalidVendor_Throws([Values(null, "")]string vendor)
        {
            var dataSource = new DataSource();
            dataSource.Vendor = vendor;
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(dataSource));
            Assert.That(ex.Message, Does.Contain("The Vendor field is required"));
        }

        [Test]
        public void Validate_InvalidName_Throws([Values(null, "")]string name)
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = name;
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(dataSource));
            Assert.That(ex.Message, Does.Contain("The Name field is required"));
        }

        [Test]
        public void Validate_DocumentTypeIsNone_Throws()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.None;
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(dataSource));
            Assert.That(ex.Message, Does.Contain("DocumentType must not be DocumentType.None"));
        }

        [Test]
        public void Validate_LocationMissing_Throws()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = null;

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(dataSource));
            Assert.That(ex.Message, Does.Contain("The Location field is required"));
        }

        [Test]
        public void Validate_InvalidDescriptor_Thows()
        {
            var dataSource = new DataSource();
            dataSource.Vendor = "vendor";
            dataSource.Name = "name";
            dataSource.DocumentType = DocumentType.Html;
            dataSource.Location = new DocumentLocator(new Request("http://test1.org"));

            dataSource.Figures.Add(new CsvDescriptor());

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(dataSource));
            Assert.That(ex.Message, Does.Contain("The Figure field is required"));
        }
    }
}
