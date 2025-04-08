import os
import shutil
from datetime import datetime
import zipfile
import sys

def create_backup():
    # Kaynak ve hedef dizinler
    source_dir = r"D:\Project\FocusLife+"
    backup_dir = r"D:\ProjectBackups\FocusLifePlus"
    
    # Hedef dizin yoksa oluştur
    if not os.path.exists(backup_dir):
        os.makedirs(backup_dir)
    
    # Yedek dosya adı için tarih-saat bilgisi
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    backup_filename = f"FocusLifePlus_backup_{timestamp}.zip"
    backup_path = os.path.join(backup_dir, backup_filename)
    
    try:
        # Zip dosyası oluştur
        with zipfile.ZipFile(backup_path, 'w', zipfile.ZIP_DEFLATED) as zipf:
            # Kaynak dizindeki tüm dosya ve klasörleri dolaş
            for root, dirs, files in os.walk(source_dir):
                # Yoksayılacak klasörler
                dirs[:] = [d for d in dirs if d not in ['.git', 'bin', 'obj', 'node_modules']]
                
                # Yoksayılacak dosya uzantıları
                files = [f for f in files if not f.endswith(('.vsidx','.log','.vscode','.vscode.*'))]
                
                for file in files:
                    # Dosya yolu
                    file_path = os.path.join(root, file)
                    # Zip içindeki relatif yol
                    arcname = os.path.relpath(file_path, source_dir)
                    # Dosyayı zip'e ekle
                    zipf.write(file_path, arcname)
        
        print(f"Yedekleme başarıyla tamamlandı: {backup_path}")
        return True
        
    except Exception as e:
        print(f"Yedekleme sırasında hata oluştu: {str(e)}")
        return False

if __name__ == "__main__":
    create_backup() 