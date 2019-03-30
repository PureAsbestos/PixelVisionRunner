﻿using System;
using System.Linq;
using System.Text;
using PixelVisionRunner.Exporters;
using PixelVisionRunner.Utils;
using PixelVisionSDK;

namespace GameCreator.Exporters
{
    public class SavedDataExporter : AbstractExporter
    {
        private IEngine targetEngine;
        private StringBuilder sb;
        
        public SavedDataExporter(string fileName, IEngine targetEngine) : base(fileName)
        {
            this.targetEngine = targetEngine;
            
            
                CalculateSteps();
        }

        public override void CalculateSteps()
        {
            if (targetEngine.gameChip.saveSlots < 1)
                return;
            
            base.CalculateSteps();
            
            // Create a new string builder
            steps.Add(CreateStringBuilder);
            
            
            steps.Add(SaveGameData);
            
            // Save the final string builder
            steps.Add(CloseStringBuilder);
        }

        private void SaveGameData()
        {
            var gameChip = targetEngine.gameChip;
            
            // Save Data
            sb.Append("\"savedData\":");

            sb.Append("{");
            
            JsonUtil.indentLevel++;
            JsonUtil.GetLineBreak(sb, 1);

            var savedData = gameChip.savedData;
            
            for (var i = savedData.Count - 1; i >= 0; i--)
            {
                var item = savedData.ElementAt(i);
                sb.Append("\"");
                sb.Append(item.Key);
                sb.Append("\": \"");
                sb.Append(item.Value);
                sb.Append("\"");
                if (i > 0)
                {
                    sb.Append(",");
                    JsonUtil.GetLineBreak(sb, 1);
                }
            }
            
            currentStep++;
        }
        
        private void CreateStringBuilder()
        {
            sb = new StringBuilder();
            
            sb.Append("{");
            JsonUtil.GetLineBreak(sb, 1);

            JsonUtil.indentLevel++;
            
            JsonUtil.GetLineBreak(sb);
            
            sb.Append("\"GameChip\":");

            JsonUtil.GetLineBreak(sb);
            sb.Append("{");
            JsonUtil.GetLineBreak(sb, 1);
            
            currentStep++;
        }

        private void CloseStringBuilder()
        {
            JsonUtil.indentLevel--;
            JsonUtil.GetLineBreak(sb, 1);
            sb.Append("}");
            
            JsonUtil.indentLevel--;
            JsonUtil.GetLineBreak(sb, 1);
            sb.Append("}");
            
            JsonUtil.GetLineBreak(sb);
            sb.Append("}");
            
            bytes = Encoding.UTF8.GetBytes(sb.ToString());
            
            currentStep++;
            
        }
    }
}