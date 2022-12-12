using FastBitmapLib;

namespace GK_Projekt2
{
    partial class Form1
    {
        private void btnStopAnimation_Click(object sender, EventArgs e)
        {
            animationInProgress = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (!animationInProgress)
            {
                int temp = Math.Min(pPictureBoxPanel.Width, pPictureBoxPanel.Height);
                pbCanvas.Width = temp;
                pbCanvas.Height = temp;
                pbCanvas.Size = new Size(temp, temp);
                _bitmap = new Bitmap(pbCanvas.Width + 2, pbCanvas.Height + 2);
                _fastBitmap = new FastBitmap(_bitmap);
                (ScaledEdgeList, ScaledVertexList, ScaledVertexOrder) = ScaleVertices(_loadedObject.FaceList, pbCanvas.Width, pbCanvas.Height);
                _filler = new Filler(_loadedObject, pbCanvas.Height, pbCanvas.Width, polySize, ScaledVertexList, ScaledVertexOrder, _texture, _normalMap, isHeightMapEnabled, _bitmap);
                _filler._texture = new Bitmap(_filler._texture, new Size(_bitmap.Width, _bitmap.Height));
                _filler._normalMap = new Bitmap(_filler._texture, new Size(_bitmap.Width, _bitmap.Height));
                SetFillerValues();
                DrawObjectAnimation();
            }
        }

        private void sbObjectR_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Io.Item1 = (double)((HScrollBar)sender).Value / 100;
            if (_filler.textureColor == false && !animationInProgress)
                DrawObject();
        }

        private void sbObjectG_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Io.Item2 = (double)((HScrollBar)sender).Value / 100;
            if (_filler.textureColor == false && !animationInProgress)
                DrawObject();
        }

        private void sbObjectB_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Io.Item3 = (double)((HScrollBar)sender).Value / 100;
            if (_filler.textureColor == false && !animationInProgress)
                DrawObject();
        }

        private void rbFixedObjectColor_CheckedChanged(object sender, EventArgs e)
        {
            _filler.textureColor = !rbFixedObjectColor.Checked;
            if (!animationInProgress)
                DrawObject();
        }

        private void btnLoadTexture_Click(object sender, EventArgs e)
        {
            importing = true;
            if (!animationInProgress)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    dialog.Filter = "Image Files|*.jpg;*.png";
                    dialog.FilterIndex = 2;
                    dialog.RestoreDirectory = true;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var filePath = dialog.FileName;
                        _texture = new Bitmap(Image.FromFile(filePath));
                        _filler._texture = new Bitmap(this._texture, new Size(_bitmap.Width, _bitmap.Height));
                        DrawObject();
                    }
                }
            }
            importing = false;
        }

        private void SetFillerValues()
        {
            _filler.textureColor = rbTextureObjectColor.Checked;
            _filler.normalVectorModified = cbModifiedNormalVector.Checked;
            _filler.light = (sbLightX.Value * pbCanvas.Height / 100, sbLightY.Value * pbCanvas.Height / 100, (int)(sbLightZ.Value * pbCanvas.Height * 1.5 / 100));
            _filler.ks = (double)(sbKs.Value / 100m);
            _filler.kd = (double)(sbKd.Value / 100m);
            _filler.m = sbm.Value;
            _filler.Il = (sbLightR.Value / 100, sbLightG.Value / 100, sbLightB.Value / 100);
            _filler.Io = (sbObjectR.Value / 100, sbObjectG.Value / 100, sbObjectB.Value / 100);
        }

        private void cbModifiedNormalVector_CheckedChanged(object sender, EventArgs e)
        {
            _filler.normalVectorModified = ((CheckBox)sender).Checked;
            if (!animationInProgress)
                DrawObject();
        }

        private void rbVectors_CheckedChanged(object sender, EventArgs e)
        {
            _filler.vectorInterpolation = ((RadioButton)sender).Checked;
            if (!animationInProgress)
                DrawObject();
        }

        private void tlpMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (!animationInProgress && !importing)
                DrawObjectAnimation();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.kd = (double)((HScrollBar)sender).Value / 100;
            if (!animationInProgress)
                DrawObject();
        }

        private void sbKs_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.ks = (double)((HScrollBar)sender).Value / 100;
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightX_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.light.Item1 = (int)(_bitmap.Height * 3 * (double)((HScrollBar)sender).Value / 100 - pbCanvas.Height);
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightY_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.light.Item2 = (int)(_bitmap.Width * 3 * (double)((HScrollBar)sender).Value / 100 - pbCanvas.Height);
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightZ_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.light.Item3 = (int)(_bitmap.Height / 2 + pbCanvas.Height * 2 * (double)((HScrollBar)sender).Value / 100);
            if (!animationInProgress)
                DrawObject();
        }

        private void sbm_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.m = (double)((HScrollBar)sender).Value;
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightR_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Il.Item1 = (double)((HScrollBar)sender).Value / 100;
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightG_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Il.Item2 = (double)((HScrollBar)sender).Value / 100;
            if (!animationInProgress)
                DrawObject();
        }

        private void sbLightB_Scroll(object sender, ScrollEventArgs e)
        {
            _filler.Il.Item3 = (double)((HScrollBar)sender).Value / 100;
            if (!animationInProgress)
                DrawObject();
        }

        private void cbMesh_CheckedChanged(object sender, EventArgs e)
        {
            if (!animationInProgress)
                DrawObject();
        }

        private async void btnAnimation_Click(object sender, EventArgs e)
        {
            if (!animationInProgress)
                await Task.Run(() => Animation());
        }

        private void btnLoadNormalMap_Click(object sender, EventArgs e)
        {
            importing = true;
            if (!animationInProgress)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    dialog.Filter = "Image Files|*.jpg;*.png";
                    dialog.FilterIndex = 2;
                    dialog.RestoreDirectory = true;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var filePath = dialog.FileName;
                        _normalMap = new Bitmap(Image.FromFile(filePath));
                        _filler._normalMap = new Bitmap(this._normalMap, new Size(_bitmap.Width, _bitmap.Height));
                        DrawObject();
                    }
                }
            }
            importing = false;
        }

        private void rbHeightMapTrue_CheckedChanged(object sender, EventArgs e)
        {
            _filler.heightMap = ((RadioButton)sender).Checked;
            this.isHeightMapEnabled = ((RadioButton)sender).Checked;
        }
    }
}
