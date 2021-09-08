﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Microsoft.ML.OnnxRuntime.Tests.Common
{
    internal static class TestDataLoader
    {
        internal static byte[] LoadModelFromEmbeddedResource(string path)
        {
            var assembly = typeof(InferenceTest).Assembly;
            byte[] model = null;

            using (Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.TestData.{path}"))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    model = memoryStream.ToArray();
                }
            }

            return model;
        }


        internal static float[] LoadTensorFromEmbeddedResource(string path)
        {
            var tensorData = new List<float>();
            var assembly = typeof(InferenceTest).Assembly;

            using (StreamReader inputFile = new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.TestData.{path}")))
            {
                inputFile.ReadLine(); //skip the input name
                string[] dataStr = inputFile.ReadLine().Split(new char[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < dataStr.Length; i++)
                {
                    tensorData.Add(Single.Parse(dataStr[i]));
                }
            }

            return tensorData.ToArray();
        }

        static void GetTypeAndWidth(Tensors.TensorElementType elemType, out Type type, out int width)
        {
            TensorElementTypeInfo result = TensorBase.GetElementTypeInfo(elemType);
            if (result != null)
            {
                type = result.TensorType;
                width = result.TypeSize;
            }
            else
            {
                type = null;
                width = 0;
            }
        }

        internal static NamedOnnxValue LoadTensorFromEmbeddedResourcePb(string path, IReadOnlyDictionary<string, NodeMetadata> nodeMetaDict)
        {
            Onnx.TensorProto tensor = null;

            var assembly = typeof(InferenceTest).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.TestData.{path}"))
            {
                tensor = Onnx.TensorProto.Parser.ParseFrom(stream);
            }

            Type tensorElemType = null;
            int width = 0;
            GetTypeAndWidth((Tensors.TensorElementType)tensor.DataType, out tensorElemType, out width);
            var intDims = new int[tensor.Dims.Count];

            for (int i = 0; i < tensor.Dims.Count; i++)
            {
                intDims[i] = (int)tensor.Dims[i];
            }

            NodeMetadata nodeMeta = null;
            string nodeName = string.Empty;

            if (nodeMetaDict.Count == 1)
            {
                nodeMeta = nodeMetaDict.Values.First();
                nodeName = nodeMetaDict.Keys.First(); // valid for single node input
            }
            else if (nodeMetaDict.Count > 1)
            {
                if (tensor.Name.Length > 0)
                {
                    nodeMeta = nodeMetaDict[tensor.Name];
                    nodeName = tensor.Name;
                }
                else
                {
                    bool matchfound = false;
                    // try to find from matching type and shape
                    foreach (var key in nodeMetaDict.Keys)
                    {
                        var meta = nodeMetaDict[key];
                        if (tensorElemType == meta.ElementType && tensor.Dims.Count == meta.Dimensions.Length)
                        {
                            int i = 0;
                            for (; i < meta.Dimensions.Length; i++)
                            {
                                if (meta.Dimensions[i] != -1 && meta.Dimensions[i] != intDims[i])
                                {
                                    break;
                                }
                            }
                            if (i >= meta.Dimensions.Length)
                            {
                                matchfound = true;
                                nodeMeta = meta;
                                nodeName = key;
                                break;
                            }
                        }
                    }
                    if (!matchfound)
                    {
                        // throw error
                        throw new Exception($"No Matching Tensor found in InputOutputMetadata corresponding to the serliazed tensor loaded from {assembly.GetName().Name}.{path}");
                    }
                }
            }
            else
            {
                // throw error
                throw new Exception($"While reading the serliazed tensor loaded from {assembly.GetName().Name}.{path}, metaDataDict has 0 elements");
            }

            if (!nodeMeta.IsTensor)
                throw new Exception("LoadTensorFromFile can load Tensor types only");

            if (tensorElemType != nodeMeta.ElementType)
                throw new Exception($"{nameof(tensorElemType)} is expected to be equal to {nameof(nodeMeta.ElementType)}");

            if (nodeMeta.Dimensions.Length != tensor.Dims.Count)
                throw new Exception($"{nameof(nodeMeta.Dimensions.Length)} is expected to be equal to {nameof(tensor.Dims.Count)}");

            for (int i = 0; i < nodeMeta.Dimensions.Length; i++)
            {
                if (!(nodeMeta.Dimensions[i] == -1) || (nodeMeta.Dimensions[i] == intDims[i]))
                    throw new Exception($"{nameof(nodeMeta.Dimensions)}[{i}] is expected to either -1 or {nameof(intDims)}[{i}]");
            }

            if (nodeMeta.ElementType == typeof(float))
            {
                return CreateNamedOnnxValueFromRawData<float>(nodeName, tensor.RawData.ToArray(), sizeof(float), intDims);
            }
            else if (nodeMeta.ElementType == typeof(double))
            {
                return CreateNamedOnnxValueFromRawData<double>(nodeName, tensor.RawData.ToArray(), sizeof(double), intDims);
            }
            else if (nodeMeta.ElementType == typeof(int))
            {
                return CreateNamedOnnxValueFromRawData<int>(nodeName, tensor.RawData.ToArray(), sizeof(int), intDims);
            }
            else if (nodeMeta.ElementType == typeof(uint))
            {
                return CreateNamedOnnxValueFromRawData<uint>(nodeName, tensor.RawData.ToArray(), sizeof(uint), intDims);
            }
            else if (nodeMeta.ElementType == typeof(long))
            {
                return CreateNamedOnnxValueFromRawData<long>(nodeName, tensor.RawData.ToArray(), sizeof(long), intDims);
            }
            else if (nodeMeta.ElementType == typeof(ulong))
            {
                return CreateNamedOnnxValueFromRawData<ulong>(nodeName, tensor.RawData.ToArray(), sizeof(ulong), intDims);
            }
            else if (nodeMeta.ElementType == typeof(short))
            {
                return CreateNamedOnnxValueFromRawData<short>(nodeName, tensor.RawData.ToArray(), sizeof(short), intDims);
            }
            else if (nodeMeta.ElementType == typeof(ushort))
            {
                return CreateNamedOnnxValueFromRawData<ushort>(nodeName, tensor.RawData.ToArray(), sizeof(ushort), intDims);
            }
            else if (nodeMeta.ElementType == typeof(byte))
            {
                return CreateNamedOnnxValueFromRawData<byte>(nodeName, tensor.RawData.ToArray(), sizeof(byte), intDims);
            }
            else if (nodeMeta.ElementType == typeof(bool))
            {
                return CreateNamedOnnxValueFromRawData<bool>(nodeName, tensor.RawData.ToArray(), sizeof(bool), intDims);
            }
            else if (nodeMeta.ElementType == typeof(Float16))
            {
                return CreateNamedOnnxValueFromRawData<Float16>(nodeName, tensor.RawData.ToArray(), sizeof(ushort), intDims);
            }
            else if (nodeMeta.ElementType == typeof(BFloat16))
            {
                return CreateNamedOnnxValueFromRawData<BFloat16>(nodeName, tensor.RawData.ToArray(), sizeof(ushort), intDims);
            }
            else
            {
                //TODO: Add support for remaining types
                throw new Exception($"Tensors of type {nameof(nodeMeta.ElementType)} not currently supporte in the LoadTensorFromEmbeddedResource");
            }
        }

        static NamedOnnxValue CreateNamedOnnxValueFromRawData<T>(string name, byte[] rawData, int elemWidth, int[] dimensions)
        {
            T[] typedArr = new T[rawData.Length / elemWidth];
            var typeOf = typeof(T);
            if (typeOf == typeof(Float16) || typeOf == typeof(BFloat16))
            {
                using (var memSrcHandle = new Memory<byte>(rawData).Pin())
                using (var memDstHandle = new Memory<T>(typedArr).Pin())
                {
                    unsafe
                    {
                        Buffer.MemoryCopy(memSrcHandle.Pointer, memDstHandle.Pointer, typedArr.Length * elemWidth, rawData.Length);
                    }
                }
            }
            else
            {
                Buffer.BlockCopy(rawData, 0, typedArr, 0, rawData.Length);
            }
            var dt = new DenseTensor<T>(typedArr, dimensions);
            return NamedOnnxValue.CreateFromTensor<T>(name, dt);
        }
    }
}