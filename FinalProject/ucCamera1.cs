using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Data.SqlClient;

namespace FinalProject
{
    public partial class ucCamera1 : UserControl
    {
        Capture capture;
        Image<Bgr, byte> ImageFrame;
        private bool captureInProgress;
        private HaarCascade haar;
        Image<Gray, byte> result;
        int ConTrains, NumLabels, t;
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");

        List<Image<Gray, byte>> trainingImage = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();

        List<string> Name = new List<string>();
        string name= null;
        public ucCamera1()
        {
            InitializeComponent();
            haar = new HaarCascade("haarcascade_frontalface_alt_tree.xml");

            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (Exception excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            if (capture != null)
            {
                if (captureInProgress)
                {
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    Application.Idle += ProcessFrame;
                }
            }
            captureInProgress = !captureInProgress;

            try
            {
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/PicNV/LabelNV.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ConTrains = NumLabels;

                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "id" + tf + ".bmp";
                    trainingImage.Add(new Image<Gray, byte>(Application.StartupPath + "/PicNV/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            }
            catch (Exception ex)
            {
               
            }
            
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageFrame = capture.QueryFrame();
            if (ImageFrame != null)
            {
                Image<Gray, byte> gray = ImageFrame.Convert<Gray, byte>();

                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(haar, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                foreach(MCvAvgComp f in facesDetected[0])
                {
                    t = t + 1;
                    result = ImageFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    ImageFrame.Draw(f.rect, new Bgr(Color.Red), 2);

                    if (trainingImage.ToArray().Length != 0)
                    {
                        MCvTermCriteria termCrit = new MCvTermCriteria(ConTrains, 0.001);

                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainingImage.ToArray(), labels.ToArray(),
                            5000, ref termCrit);

                        name = recognizer.Recognize(result);
                      
                       if(txtID.Text!=name)
                       {
                            txtID.DataBindings.Clear();
                            txtTen.DataBindings.Clear();
                            txtChucvu.DataBindings.Clear();
                            txtDiachi.DataBindings.Clear();
                            txtGioitinh.DataBindings.Clear();
                            txtSoDT.DataBindings.Clear();
                            txtNgaysinh.DataBindings.Clear();                   
                      
                            ShowInfo(name);
                            MessageBox.Show("Đã ghi nhận giờ vào nhân viên có "+txtID.Text);
                        }
                    }
                }
            }
            imageBox1.Image = ImageFrame;
        }
        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void ShowInfo(string id)
        {
            con.Open();
            string sql = "select * from Employees where ID="+id;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            

                txtID.DataBindings.Add("Text", dt, "ID");
                txtTen.DataBindings.Add("Text", dt, "LastName");
                txtNgaysinh.DataBindings.Add("Text", dt, "Birthday");
                txtGioitinh.DataBindings.Add("Text", dt, "Sex");
                txtSoDT.DataBindings.Add("Text", dt, "Phone");
                txtDiachi.DataBindings.Add("Text", dt, "Address");
                txtChucvu.DataBindings.Add("Text", dt, "Position");
                pictureBox1.ImageLocation = Application.StartupPath + "/PicNV/id" + id + ".bmp";
                label10.Text = DateTime.Now.ToString();

            string sql1 = "if not exists(select EmployeesID from TimeWorking where EmployeesID='" + txtID.Text + "' and Date='" + DateTime.Now.ToShortDateString() + "') insert into TimeWorking (EmployeesID,Date,TimeIn) values ('" + txtID.Text + "','" + DateTime.Now + "','" + DateTime.Now + "')";
            com = new SqlCommand(sql1, con);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
