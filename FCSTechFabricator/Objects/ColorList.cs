﻿using System.Collections.Generic;
using FCSCommon.Utilities;
using FCSTechFabricator.Extensions;
using UnityEngine;

namespace FCSTechFabricator.Objects
{
    public static class ColorList
    {
        public static List<ColorVec4> Colors = new List<ColorVec4>
        {
            new ColorVec4(0f,0f,0f,1f),
            new ColorVec4(0.827451f,1f,0.9921569f,1f),
            new ColorVec4(0.9568628f,0.764706f,0f,1f),
            new ColorVec4(0.8666667f,0.1960784f,0.1254902f,1f),
            new ColorVec4(1f,1f,1f,1f),
            new ColorVec4(0.2784314f,0.8509805f,0f,1f),
            new ColorVec4(0f,0.3215686f,0.8509805f,1f),
            new ColorVec4(0.509804f,0.01568628f,0.9333334f,1f),
            new ColorVec4(0.8039216f,0.3607843f,0.3607843f,1f),
            new ColorVec4(0.9411765f,0.5019608f,0.5019608f,1f),
            new ColorVec4(0.9803922f,0.5019608f,0.4470588f,1f),
            new ColorVec4(0.9137255f,0.5882353f,0.4784314f,1f),
            new ColorVec4(1f,0.627451f,0.4784314f,1f),
            new ColorVec4(0.8627451f,0.07843138f,0.2352941f,1f),
            new ColorVec4(1f,0f,0f,1f),
            new ColorVec4(0.6980392f,0.1333333f,0.1333333f,1f),
            new ColorVec4(0.5450981f,0f,0f,1f),
            new ColorVec4(1f,0.7529412f,0.7960784f,1f),
            new ColorVec4(1f,0.7137255f,0.7568628f,1f),
            new ColorVec4(1f,0.4117647f,0.7058824f,1f),
            new ColorVec4(1f,0.07843138f,0.5764706f,1f),
            new ColorVec4(0.7803922f,0.08235294f,0.5215687f,1f),
            new ColorVec4(0.8588235f,0.4392157f,0.5764706f,1f),
            new ColorVec4(1f,0.627451f,0.4784314f,1f),
            new ColorVec4(1f,0.4980392f,0.3137255f,1f),
            new ColorVec4(1f,0.3882353f,0.2784314f,1f),
            new ColorVec4(1f,0.2705882f,0f,1f),
            new ColorVec4(1f,0.5490196f,0f,1f),
            new ColorVec4(1f,0.6470588f,0f,1f),
            new ColorVec4(1f,0.8431373f,0f,1f),
            new ColorVec4(1f,1f,0f,1f),
            new ColorVec4(1f,1f,0.8784314f,1f),
            new ColorVec4(1f,0.9803922f,0.8039216f,1f),
            new ColorVec4(0.9803922f,0.9803922f,0.8235294f,1f),
            new ColorVec4(1f,0.9372549f,0.8352941f,1f),
            new ColorVec4(1f,0.8941177f,0.7098039f,1f),
            new ColorVec4(1f,0.854902f,0.7254902f,1f),
            new ColorVec4(0.9333333f,0.9098039f,0.6666667f,1f),
            new ColorVec4(0.9411765f,0.9019608f,0.5490196f,1f),
            new ColorVec4(0.7411765f,0.7176471f,0.4196078f,1f),
            new ColorVec4(0.9019608f,0.9019608f,0.9803922f,1f),
            new ColorVec4(0.8470588f,0.7490196f,0.8470588f,1f),
            new ColorVec4(0.8666667f,0.627451f,0.8666667f,1f),
            new ColorVec4(0.9333333f,0.509804f,0.9333333f,1f),
            new ColorVec4(0.854902f,0.4392157f,0.8392157f,1f),
            new ColorVec4(1f,0f,1f,1f),
            new ColorVec4(1f,0f,1f,1f),
            new ColorVec4(0.7294118f,0.3333333f,0.827451f,1f),
            new ColorVec4(0.5764706f,0.4392157f,0.8588235f,1f),
            new ColorVec4(0.4f,0.2f,0.6f,1f),
            new ColorVec4(0.5411765f,0.1686275f,0.8862745f,1f),
            new ColorVec4(0.5803922f,0f,0.827451f,1f),
            new ColorVec4(0.6f,0.1960784f,0.8f,1f),
            new ColorVec4(0.5450981f,0f,0.5450981f,1f),
            new ColorVec4(0.5019608f,0f,0.5019608f,1f),
            new ColorVec4(0.2941177f,0f,0.509804f,1f),
            new ColorVec4(0.4156863f,0.3529412f,0.8039216f,1f),
            new ColorVec4(0.282353f,0.2392157f,0.5450981f,1f),
            new ColorVec4(0.4823529f,0.4078431f,0.9333333f,1f),
            new ColorVec4(0.6784314f,1f,0.1843137f,1f),
            new ColorVec4(0.4980392f,1f,0f,1f),
            new ColorVec4(0.4862745f,0.9882353f,0f,1f),
            new ColorVec4(0f,1f,0f,1f),
            new ColorVec4(0.1960784f,0.8039216f,0.1960784f,1f),
            new ColorVec4(0.5960785f,0.9843137f,0.5960785f,1f),
            new ColorVec4(0.5647059f,0.9333333f,0.5647059f,1f),
            new ColorVec4(0f,0.9803922f,0.6039216f,1f),
            new ColorVec4(0f,1f,0.4980392f,1f),
            new ColorVec4(0.2352941f,0.7019608f,0.4431373f,1f),
            new ColorVec4(0.1803922f,0.5450981f,0.3411765f,1f),
            new ColorVec4(0.1333333f,0.5450981f,0.1333333f,1f),
            new ColorVec4(0f,0.5019608f,0f,1f),
            new ColorVec4(0f,0.3921569f,0f,1f),
            new ColorVec4(0.6039216f,0.8039216f,0.1960784f,1f),
            new ColorVec4(0.4196078f,0.5568628f,0.1372549f,1f),
            new ColorVec4(0.5019608f,0.5019608f,0f,1f),
            new ColorVec4(0.3333333f,0.4196078f,0.1843137f,1f),
            new ColorVec4(0.4f,0.8039216f,0.6666667f,1f),
            new ColorVec4(0.5607843f,0.7372549f,0.5450981f,1f),
            new ColorVec4(0.1254902f,0.6980392f,0.6666667f,1f),
            new ColorVec4(0f,0.5450981f,0.5450981f,1f),
            new ColorVec4(0f,0.5019608f,0.5019608f,1f),
            new ColorVec4(0f,1f,1f,1f),
            new ColorVec4(0f,1f,1f,1f),
            new ColorVec4(0.8784314f,1f,1f,1f),
            new ColorVec4(0.6862745f,0.9333333f,0.9333333f,1f),
            new ColorVec4(0.4980392f,1f,0.8313726f,1f),
            new ColorVec4(0.2509804f,0.8784314f,0.8156863f,1f),
            new ColorVec4(0.282353f,0.8196079f,0.8f,1f),
            new ColorVec4(0f,0.8078431f,0.8196079f,1f),
            new ColorVec4(0.372549f,0.6196079f,0.627451f,1f),
            new ColorVec4(0.2745098f,0.509804f,0.7058824f,1f),
            new ColorVec4(0.6901961f,0.7686275f,0.8705882f,1f),
            new ColorVec4(0.6901961f,0.8784314f,0.9019608f,1f),
            new ColorVec4(0.6784314f,0.8470588f,0.9019608f,1f),
            new ColorVec4(0.5294118f,0.8078431f,0.9215686f,1f),
            new ColorVec4(0.5294118f,0.8078431f,0.9803922f,1f),
            new ColorVec4(0f,0.7490196f,1f,1f),
            new ColorVec4(0.1176471f,0.5647059f,1f,1f),
            new ColorVec4(0.3921569f,0.5843138f,0.9294118f,1f),
            new ColorVec4(0.4823529f,0.4078431f,0.9333333f,1f),
            new ColorVec4(0.254902f,0.4117647f,0.8823529f,1f),
            new ColorVec4(0f,0f,1f,1f),
            new ColorVec4(0f,0f,0.8039216f,1f),
            new ColorVec4(0f,0f,0.5450981f,1f),
            new ColorVec4(0f,0f,0.5019608f,1f),
            new ColorVec4(0.09803922f,0.09803922f,0.4392157f,1f),
            new ColorVec4(1f,0.972549f,0.8627451f,1f),
            new ColorVec4(1f,0.9215686f,0.8039216f,1f),
            new ColorVec4(1f,0.8941177f,0.7686275f,1f),
            new ColorVec4(1f,0.8705882f,0.6784314f,1f),
            new ColorVec4(0.9607843f,0.8705882f,0.7019608f,1f),
            new ColorVec4(0.8705882f,0.7215686f,0.5294118f,1f),
            new ColorVec4(0.8235294f,0.7058824f,0.5490196f,1f),
            new ColorVec4(0.7372549f,0.5607843f,0.5607843f,1f),
            new ColorVec4(0.9568627f,0.6431373f,0.3764706f,1f),
            new ColorVec4(0.854902f,0.6470588f,0.1254902f,1f),
            new ColorVec4(0.7215686f,0.5254902f,0.04313726f,1f),
            new ColorVec4(0.8039216f,0.5215687f,0.2470588f,1f),
            new ColorVec4(0.8235294f,0.4117647f,0.1176471f,1f),
            new ColorVec4(0.5450981f,0.2705882f,0.07450981f,1f),
            new ColorVec4(0.627451f,0.3215686f,0.1764706f,1f),
            new ColorVec4(0.6470588f,0.1647059f,0.1647059f,1f),
            new ColorVec4(0.5019608f,0f,0f,1f),
            new ColorVec4(1f,0.9803922f,0.9803922f,1f),
            new ColorVec4(0.9411765f,1f,0.9411765f,1f),
            new ColorVec4(0.7529412f,0.7529412f,0.7529412f,1f),
            new ColorVec4(0.6627451f,0.6627451f,0.6627451f,1f),
            new ColorVec4(0.5019608f,0.5019608f,0.5019608f,1f),
            new ColorVec4(0.4117647f,0.4117647f,0.4117647f,1f),
            new ColorVec4(0.4666667f,0.5333334f,0.6f,1f),
            new ColorVec4(0.4392157f,0.5019608f,0.5647059f,1f),
            new ColorVec4(0.1843137f,0.3098039f,0.3098039f,1f),
            new ColorVec4(0.3f,0.3f,0.3f,1f),
        };

        public static void AddColor(Color color)
        {
            var vec4Color = color.ColorToVector4();
            var match = false;

            foreach (ColorVec4 colorVec4 in Colors)
            {
                if (!colorVec4.Compare(vec4Color)) continue;
                match = true;
                break;
            }
            
            if (!match)
            {
                Colors.Add(vec4Color);
                QuickLogger.Info($"Added new color to ColorsList: {color}");
            }
        }
    }
}
