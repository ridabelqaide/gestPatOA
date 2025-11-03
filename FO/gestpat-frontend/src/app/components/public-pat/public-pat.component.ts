import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule,NgForm } from '@angular/forms';
import { PublicPatService } from '../../Services/public-pat.service';
import { PublicPat } from '../../models/public-pat.model';
import { Document, Packer, Paragraph, Table, TableCell, TableRow, TextRun, AlignmentType, BorderStyle, VerticalAlign } from "docx";
import { saveAs } from "file-saver";
import Swal from 'sweetalert2';

@Component({
  selector: 'app-public-pat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './public-pat.component.html',
})
export class PublicPatComponent implements OnInit {

  publicPats: PublicPat[] = [];
  selectedPat?: PublicPat;
  isDetailModalOpen = false;
  isFormModalOpen = false;
  isEditMode = false;
  formData: PublicPat = new PublicPat();
  searchMatricule: string = '';

  constructor(private publicPatService: PublicPatService) { }

  ngOnInit(): void {
    this.getAll();
  }

  getAll(): void {
    this.publicPatService.getAll(this.searchMatricule).subscribe(
      data => this.publicPats = data,
      err => console.error(err)
    );
  }

  onSearch(): void {
    this.getAll();
  }

  openDetail(p: PublicPat) {
    this.selectedPat = p;
    this.isDetailModalOpen = true;
  }

  closeDetail() {
    this.isDetailModalOpen = false;
    this.selectedPat = undefined;
  }

  openForm(p?: PublicPat) {
    if (p) {
      this.isEditMode = true;
      this.formData = { ...p };
    } else {
      this.isEditMode = false;
      this.formData = new PublicPat();
    }
    this.isFormModalOpen = true;
  }

  closeForm() {
    this.isFormModalOpen = false;
    this.formData = new PublicPat();
  }

  save() {
    if (this.isEditMode) {
      this.publicPatService.update(this.formData.id!, this.formData).subscribe({
        next: () => {
          this.getAll();
          this.closeForm();
          Swal.fire({
            icon: 'success',
            title: 'تم التعديل بنجاح ',
            text: 'تم تحديث بيانات الملك العمومي بنجاح',
            confirmButtonText: 'موافق',
          });
        }
      });
    } else {
      this.publicPatService.create(this.formData).subscribe({
        next: () => {
          this.getAll();
          this.closeForm();
          Swal.fire({
            icon: 'success',
            title: 'تمت الإضافة بنجاح ',
            text: 'تمت إضافة ملك عمومي جديد بنجاح',
            confirmButtonText: 'موافق',
          });
        }
      });
    }
  }
  onSubmit(f: NgForm) {
    if (f.invalid) {
      Object.values(f.controls).forEach(control => control.markAsTouched());
      Swal.fire({
        icon: 'warning',
        title: 'تحقق من الحقول',
        text: 'يرجى ملء جميع الحقول المطلوبة قبل المتابعة',
        confirmButtonText: 'حسنًا'
      });
      return;
    }
    this.save();
  }

  delete(p: PublicPat) {
    Swal.fire({
      title: `هل أنت متأكد من حذف الملك "${p.registrationNumber}"؟`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، احذف',
      cancelButtonText: 'إلغاء',
      reverseButtons: true
    }).then((result) => {
      if (result.isConfirmed) {
        this.publicPatService.delete(p.id!).subscribe({
          next: () => {
            this.getAll();
            Swal.fire({
              icon: 'success',
              title: 'تم الحذف بنجاح ',
              text: 'تم حذف الملك العمومي بنجاح',
              confirmButtonText: 'موافق'
            });
          }
        });
      }
    });
  }

  exportToWord(publicPats: PublicPat[]) {
    if (!publicPats || publicPats.length === 0) return;

    const headers = [
      'ملاحظات','تاريخ إخراج العقار','مرجع إخراج العقار من الملك العام (في حالة الإخراج)',
      'أجال و كيفيات أداء إتاوة الاحتلال المؤقت','نسبة الزيادة','مبلغ إتاوة الاحتلال','مدة إتاوة الاحتلال',
      'مدة الاحتلال المؤقت', 'الشخص المرخص له بالاحتلال المؤقت(أو المرخص لهم)','تاريخ إبرام سند الترخيص بالاستغلال:',
      'الاستغلال الخصوصي من طرف الغير','القيمة التداولية للعقـار','الاستعمال الفعلي','التخصيص حسب وثائق التعمير',
      'الأساس القانوني المعتمد لترتيب العقار ضمن الأملاك العامة','ثمن الاقتناء', 'مصدر تملك العقــار','الإحداثيات الجغرافية', 'الموقع',
      'المساحة','المرجع و تاريخ تسجيله بإدارة التسجيل','المراجع العقارية','النـــــــــــــوع','رقم التقييد في السجل'
    ];


    // Ligne d'en-tête
    const headerRow = new TableRow({
      children: headers.map(h => new TableCell({
        children: [new Paragraph({ children: [new TextRun({ text: h, bold: true, size: 24 })] })],
        verticalAlign: VerticalAlign.CENTER,
        borders: {
          top: { style: BorderStyle.SINGLE },
          bottom: { style: BorderStyle.SINGLE },
          left: { style: BorderStyle.SINGLE },
          right: { style: BorderStyle.SINGLE },
        },
        shading: { fill: 'D9D9D9' },
      }))
    });

    // Lignes de données
    const dataRows = publicPats.map(p => new TableRow({
      children: [
        new TableCell({ children: [new Paragraph(String(p.notesAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.removalFromPublicDomainDate ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.removalFromPublicDomainReference ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.paymentDeadlinesAndMethodsAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.increaseRatePercent ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.occupationFeeAmount ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.occupationFeeDuration ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.temporaryOccupationDuration ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.authorizedPersonAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.occupationPermitDate ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.privateUseDetailsAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.marketValue ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.currentUseAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.zoningDesignationAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.legalBasisAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.purchasePrice ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.acquisitionSourceAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(`العرض: ${p.latitude ?? ''}, الطول: ${p.longitude ?? ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.locationAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.area ?? ''))] }),
        new TableCell({ children: [new Paragraph(`${p.registrationReference ?? ''} / ${p.registrationDate ?? ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.landReferencesAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.typeAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.registrationNumber ?? ''))] }),
      ]
    }));

    const table = new Table({ rows: [headerRow, ...dataRows] });

    const doc = new Document({
      sections: [
        {
          children: [
            new Paragraph({
              children: [new TextRun({ text: 'قائمة الأملاك العامة', bold: true, size: 48 })],
              alignment: AlignmentType.CENTER
            }),
            new Paragraph({ text: '' }),
            table
          ]
        }
      ]
    });

    Packer.toBlob(doc).then(blob => saveAs(blob, 'قائمة_الأملاك_العامة.docx'));
  }
}
