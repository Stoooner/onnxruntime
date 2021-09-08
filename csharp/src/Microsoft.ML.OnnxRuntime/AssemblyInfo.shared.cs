#if __IOS__
[assembly: Foundation.LinkerSafe]
#elif __ANDROID__
[assembly: Android.LinkerSafe]
#endif

// Making this assembly's internals visible to the internal Test assembly
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.ML.OnnxRuntime.Tests," +
                              "PublicKey=002400000480000094000000060200000024000052534131000400000100010059013e94e4bc70" +
                              "136ca4c35f33acd6b62974536b698f9c7a21cee18d805c7ad860ad9eebfdc47a96ba2f8d03f4cf" +
                              "1c36b9d30787e276c7b9833b5bf2a6eba7e919e6b90083078a352262aed1d842e5f70a3085cbcf" +
                              "4c56ae851b161137920961c23fcc246598d61d258ccc615c927b2441359eea666a99ce1c3c07dc" +
                              "a18fb0e1")]

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.ML.OnnxRuntime.Tests.Common, PublicKey=002400000480000094000000060200000024000052534131000400000100010059013e94e4bc70136ca4c35f33acd6b62974536b698f9c7a21cee18d805c7ad860ad9eebfdc47a96ba2f8d03f4cf1c36b9d30787e276c7b9833b5bf2a6eba7e919e6b90083078a352262aed1d842e5f70a3085cbcf4c56ae851b161137920961c23fcc246598d61d258ccc615c927b2441359eea666a99ce1c3c07dca18fb0e1")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.ML.OnnxRuntime.Tests.iOS, PublicKey=002400000480000094000000060200000024000052534131000400000100010059013e94e4bc70136ca4c35f33acd6b62974536b698f9c7a21cee18d805c7ad860ad9eebfdc47a96ba2f8d03f4cf1c36b9d30787e276c7b9833b5bf2a6eba7e919e6b90083078a352262aed1d842e5f70a3085cbcf4c56ae851b161137920961c23fcc246598d61d258ccc615c927b2441359eea666a99ce1c3c07dca18fb0e1")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.ML.OnnxRuntime.Tests.Droid, PublicKey=002400000480000094000000060200000024000052534131000400000100010059013e94e4bc70136ca4c35f33acd6b62974536b698f9c7a21cee18d805c7ad860ad9eebfdc47a96ba2f8d03f4cf1c36b9d30787e276c7b9833b5bf2a6eba7e919e6b90083078a352262aed1d842e5f70a3085cbcf4c56ae851b161137920961c23fcc246598d61d258ccc615c927b2441359eea666a99ce1c3c07dca18fb0e1")]