﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NnCase.Converter.Model.Layers;
using NnCase.Converter.Model.Layers.K210;
using ReactiveUI;

namespace NnCase.Designer.Modules.ModelDesigner.ViewModels.Layers.K210
{
    public enum K210Stride
    {
        [Display(Name = "1x1")]
        Stride1x1,
        [Display(Name = "2x2")]
        Stride2x2
    }

    public enum K210Conv2dKernelSize
    {
        [Display(Name = "1x1")]
        Size1x1,
        [Display(Name = "3x3")]
        Size3x3
    }

    public class K210Conv2dViewModel : LayerViewModel<K210Conv2d>
    {
        public InputConnectorViewModel Input { get; }

        public OutputConnectorViewModel Output { get; }

        private ActivationFunctionType _activation;
        public ActivationFunctionType Activation
        {
            get => _activation;
            set
            {
                if (_activation != value)
                {
                    _activation = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private K210Stride _stride;
        public K210Stride Stride
        {
            get => _stride;
            set
            {
                if (_stride != value)
                {
                    _stride = value;
                    this.RaisePropertyChanged();
                    UpdateOutput();
                }
            }
        }

        private K210Conv2dKernelSize _kernelSize;
        public K210Conv2dKernelSize KernelSize
        {
            get => _kernelSize;
            set
            {
                if (_kernelSize != value)
                {
                    _kernelSize = value;
                    this.RaisePropertyChanged();
                    UpdateOutput();
                }
            }
        }

        private int _outputChannels = 1;
        public int OutputChannels
        {
            get => _outputChannels;
            set
            {
                if (_outputChannels != value)
                {
                    _outputChannels = value;
                    this.RaisePropertyChanged();
                    UpdateOutput();
                }
            }
        }

        public K210Conv2dViewModel()
        {
            Input = AddInput("input");
            Input.Updated += Input_Updated;
            Output = AddOutput("output", new[] { 1, 1, 1, 1 });
        }

        private void Input_Updated(object sender, EventArgs e)
        {
            UpdateOutput();
        }

        private void UpdateOutput()
        {
            var input = Input.Connection?.From;
            if (input != null)
            {
                var stride = Stride == K210Stride.Stride1x1 ? 1 : 2;
                Output.SetDimension(d =>
                {
                    d[1] = OutputChannels;
                    d[2] = input.Dimensions[2] / stride;
                    d[3] = input.Dimensions[3] / stride;
                });
            }
        }
    }
}