using System;
using System.Collections.Generic;
using System.Linq;

using Grobid.NET.Contract;

// XXX: this is a work in progress, the only thing to see here are
// (dis)organized thoughts.

namespace Grobid.NET.Feature.Date
{
    // This is a rough sketch of my intention...  The names are purposefully
    // ridiculuous because they have no meaning.
    // 
    // Given an X we want to extract metadata about it.  Extracting metadata
    // requires several different concepts, but from the end user's
    // persepective they supply X and get a type representing the metadata
    // for X.  This is the contract.
    //
    // interface IEntity<T> {
    //     T Create(BlockState[] blockState);
    // }
    //
    // In reality the user cannot supply an arbitrary X, they have to supply
    // a PDF document.  A PDF document gets parsed to BlockState[].
    //
    // ## AppleSauceFeatureVectorFactory
    // Every block state is converted to a feature vector.  A feature vector
    // provides metadata about each block state.  e.g. does the block state
    // represent a number, or is it all capital letters.  Feature vectors are
    // determined ahead of time, and without user's input.  Better features
    // lead to better extraction.
    //
    // ## AppleSauceFormatter
    // A feature vector is converted to a list of key/value pairs.  (This step
    // may be unnecessary.)  I think formatters are particular to a CRP engine,
    // and can be re-used for all of the models implemented for a given engine.
    // This is all an implementation detail, so ...
    //
    // ## WapitiFeatureVectorTransformer
    // key/value pairs are formatted for processing by the target CRP library.
    // In the case of Wapiti feature values are separated by a space.  A feature
    // vector's key/value pairs are aggregated, and separated by a newline.  The
    // feature names are not kept for Wapiti.
    //
    // The intent of emitting a key/value pair is to easily identify what field
    // (key) a value is associated with.  When a feature vector is of the form
    // 1 0 1 BLOCKIN 1 0 1 0 1 1 it is *very* difficult to tell what values map to
    // what field.
    //
    // I don't know if emitting a key/value pair can be used to combat the issue of
    // determining column to name mapping; therefore, I do not know how valuable this
    // is.  I do know it is a lot of extra baggage to carry.
    //
    // ## AppleSauceLabeler
    // The key/value pair "document" is then processed by the labeler (e.g. Wapiti).
    //
    // Wapiti returns a nearly identical document as was supplied with the addition
    // of labels added to each line.  Labels are of the form I-<label> or <label>
    // and appear at the end of a line.  They are the last field, and are separated
    // by a '\t' as opposed to a space ' ' in the case of Wapiti.
    //
    // The labeled document is parsed to extract the original block state value,
    // Wapiti's assigned label, and if it is a starting label or not, i.e. I-<label>
    // vs. <label>.  Each line is extracted to a FeatureRow, which represents the
    // aforementioned state.
    //
    // ## AppleSauceEntityFactory
    // The feature rows are transformed to an entity.  The entity is a type
    // representing metadata extracted from the PDF.
    //
    // ## AppleEngine
    // An Engine represents a CRP engine, and all of the entities it is capable
    // of extracting from a PDF document, e.g.
    //  * Date
    //  * Header
    //  * Name
    //
    // The interface IEngine establishes the contract that all CRP Engine instances
    // must implement.

    public sealed class WapitiFeatureVectorTransformer
    {
        public string Transform(KeyValuePair<string, string>[] features)
        {
            throw new NotImplementedException();
        }
    }

    public interface IEngine
    {
        //Entity.Header ExtractHeader(BlockState[] blockStates);
        //Entity.Date ExtractDate(BlockState[] blockStates);
        //Entity.Name ExtractName(BlockState[] blockStates);
    }

    public class AppleEngine : IEngine
    {
    }

    public class AppleSauce : IModel<AppleSauceEntity>
    {
        private AppleSauceFormatter formatter;
        private AppleSauceFeatureVectorFactory factory;
        private WapitiFeatureVectorTransformer transformer;
        private AppleSauceLabeler labeler;
        private AppleSauceEntityFactory entityFactory;

        public AppleSauce()
        {
            this.factory = new AppleSauceFeatureVectorFactory();
            this.entityFactory = new AppleSauceEntityFactory();
            this.formatter = new AppleSauceFormatter();
            this.transformer = new WapitiFeatureVectorTransformer();
            this.labeler = new AppleSauceLabeler();
        }

        public AppleSauceEntity Create(BlockState[] blockStates)
        {
            var featureVectorLines = blockStates
                .Select(x => this.factory.Create(x))
                .Select(x => this.formatter.Format(x))
                .Select(x => this.transformer.Transform(x));

            var featureVectorDocument = String.Join(Environment.NewLine, featureVectorLines);
            var featureRows = this.labeler.Label(featureVectorDocument);
            var model = this.entityFactory.Create(featureRows);
            return model;
        }
    }

    // 1. PDF Blocks              -> AppleSauceFeatureVector   # AppleSauceFactory
    // 2. AppleSauceFeatureVector -> string[]     # AppleSauceFormatter
    // 2.a. string[] -> IEnumerable<string[]>
    // 3. IEnumerable<string[]>   -> FeatureRow[] # AppleSauceLabeler
    // 4. FeatureRow[]            -> AppleSauceEntity

    public class AppleSauceLabeler
    {
        public FeatureRow[] Label(string document)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFormatter
    {
        public KeyValuePair<string, string>[] Format(AppleSauceFeatureVector appleSauce)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFeatureVectorFactory : IFeatureVectorFactory<AppleSauceFeatureVector>
    {
        public AppleSauceFeatureVector Create(BlockState blockState)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFeatureVector
    {
    }

    public class AppleSauceEntityFactory
    {
        public AppleSauceEntity Create(FeatureRow[] featureRows)
        {
            throw new NotImplementedException();
        }
    }

    public class AuthorEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Affiliation { get; set; }
    }

    public class AppleSauceEntity
    {
        public string Title { get; set; }
        public AuthorEntity[] Authors { get; set; }
        public string[] Keywords { get; set; }
        public string Abstract { get; set; }
    }
}
