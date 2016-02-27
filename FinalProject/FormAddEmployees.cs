using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class FormAddEmployees : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        Capture capture;
        Image<Bgr, byte> ImageFrame;
        private bool captureInProgress;
        private HaarCascade haar;

        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImage = new List<Image<Gray, byte>>();

        int ConTrains;
        List<string> labels = new List<string>();

        string tenanh="";


        public FormAddEmployees()
        {
            InitializeComponent();

            haar = new HaarCascade("haarcascade_frontalface_alt_tree.xml");

            capture = new Capture();
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
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageFrame = capture.QueryFrame();
            gray = ImageFrame.Convert<Gray, byte>();

            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(haar, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));
            foreach (MCvAvgComp f in facesDetected[0])
            {
                result = ImageFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                ImageFrame.Draw(f.rect, new Bgr(Color.Red), 2);
            }

            imgCam.Image = ImageFrame;
        }
        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void FormAddEmployees_Load(object sender, EventArgs e)
        {
            rdbtnNam.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtDate.DateTime>=DateTime.Now)
            {
                MessageBox.Show("Ngày tháng không phù hợp!");
                return;
            }
            if(txtID.Text==""|| txtHo.Text==""||txtChucvu.Text==""||txtDate.Text==""||txtDiachi.Text==""||txtSDT.Text==""||txtTen.Text=="")
            {
                MessageBox.Show("Thông tin chưa đầy đủ! Vui lòng nhập đầy đủ thông tin nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if(!System.IO.Directory.Exists(Application.StartupPath+"/PicNV/"))
                    System.IO.Directory.CreateDirectory(Application.StartupPath+"/PicNV/");

                ConTrains++;

                gray = capture.QueryGrayFrame().Resize(320, 204, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                MCvAvgComp[][] facesDeteced = gray.DetectHaarCascade(haar, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));

                foreach (MCvAvgComp f in facesDeteced[0])
                {
                    TrainedFace = ImageFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImage.Add(TrainedFace);
                labels.Add(txtID.Text);

                imgNV.Image = TrainedFace;

                File.WriteAllText(Application.StartupPath + "/PicNV/LabelNV.txt", trainingImage.ToArray().Length.ToString() + "%");

                for (int i = 1; i < trainingImage.ToArray().Length + 1; i++)
                {
                    trainingImage.ToArray()[i - 1].Save(Application.StartupPath + "/PicNV/id" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/PicNV/LabelNV.txt", labels.ToArray()[i - 1] + "%");
                }

                Add();

               
                MessageBox.Show("Thêm nhân viên "+txtHo.Text+" "+ txtTen.Text + " thành công!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Open face detection!", "Fail!",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        private void Add()
        {
            string sex="Nam";
            if(rdbtnNu.Checked==true)
            {
                sex="Nữ";
            }
            con.Open();
            string sql = "insert into Employees(ID,FirstName,LastName,Birthday,Sex,Position,Phone,Address) values ('"+txtID.Text+"','"+txtHo.Text+"','"+txtTen.Text+"','"+txtDate.DateTime.ToShortDateString()+"','"+sex.ToString()+"','"+txtChucvu.Text+"','"+txtSDT.Text+"','"+txtDiachi.Text+"')";
            SqlCommand com = new SqlCommand(sql, con);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
