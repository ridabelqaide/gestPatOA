import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import Swal from 'sweetalert2';
import { PrivatePat } from '../../models/private-pat.model';
import { PrivatePatService } from '../../Services/private-pat.service';
import {
  Document, Packer, Paragraph, Table, TableCell, TableRow,
  TextRun, AlignmentType, BorderStyle, VerticalAlign
} from "docx";
import { saveAs } from "file-saver";

@Component({
  selector: 'app-private-pat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './private-pat.component.html',
  styleUrls: ['./private-pat.component.css']
})
export class PrivatePatComponent implements OnInit {

  privatePats: PrivatePat[] = [];
  selectedPat: PrivatePat | null = null;
  searchMatricule: string = '';
  formData: PrivatePat = new PrivatePat();
  isFormModalOpen = false;
  isDetailModalOpen = false;
  isEditMode = false;

  constructor(private privatePatService: PrivatePatService) { }

  ngOnInit() {
    this.loadPrivatePats();
  }

  loadPrivatePats(search: string = ''): void {
    this.privatePatService.getAll(search).subscribe({
      next: (data) => this.privatePats = data,
      error: (err) => console.error(err)
    });
  }

  onSearch(): void {
    const searchValue = this.searchMatricule?.trim() || '';
    this.loadPrivatePats(searchValue);
  }

  openDetail(p: PrivatePat) {
    this.selectedPat = p;
    this.isDetailModalOpen = true;
  }

  closeDetail() {
    this.selectedPat = null;
    this.isDetailModalOpen = false;
  }

  openForm(p?: PrivatePat) {
    if (p) {
      this.formData = { ...p };
      this.isEditMode = true;
    } else {
      this.formData = new PrivatePat();
      this.isEditMode = false;
    }
    this.isFormModalOpen = true;
  }

  closeForm() {
    this.isFormModalOpen = false;
    this.formData = new PrivatePat();
  }

  submitForm(form: NgForm) {
    if (form.invalid) {
      Object.values(form.controls).forEach(c => c.markAsTouched());
      Swal.fire({
        icon: 'error',
        title: 'تحقق من الحقول المطلوبة',
        text: 'الرجاء ملء جميع الحقول الإلزامية قبل الحفظ',
        confirmButtonText: 'موافق'
      });
      return;
    }

    if (this.isEditMode) {
      this.privatePatService.update(this.formData.id!, this.formData).subscribe({
        next: () => {
          this.loadPrivatePats();
          this.closeForm();
          Swal.fire({
            icon: 'success',
            title: 'تم التعديل بنجاح',
            text: 'تم تحديث بيانات الملك الخاص بنجاح',
            confirmButtonText: 'موافق'
          });
        }
      });
    } else {
      this.privatePatService.create(this.formData).subscribe({
        next: () => {
          this.loadPrivatePats();
          this.closeForm();
          Swal.fire({
            icon: 'success',
            title: 'تمت الإضافة بنجاح',
            text: 'تمت إضافة ملك خاص جديد بنجاح',
            confirmButtonText: 'موافق'
          });
        }
      });
    }
  }

  delete(p: PrivatePat) {
    Swal.fire({
      title: 'تأكيد الحذف',
      text: 'هل تريد حذف هذا الملك الخاص؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، احذف',
      cancelButtonText: 'إلغاء'
    }).then(result => {
      if (result.isConfirmed && p.id) {
        this.privatePatService.delete(p.id).subscribe(() => {
          this.loadPrivatePats();
          Swal.fire('تم الحذف', 'تم حذف الملك الخاص بنجاح', 'success');
        });
      }
    });
  }

  exportToWord(privatePats: PrivatePat[]) {
    if (!privatePats || privatePats.length === 0) return;

    const headers = [
      ' رقم التقييد في السجل:', 'النـــــــــــــوع', 'المراجع العقارية للملك',
      'المرجع و تاريخ تسجيله بإدارة التسجيل', 'المساحة', 'الموقع', ' الإحداثيات الجغرافية',
      ' مصدر تملك العقــار', 'ثمن الاقتناء', 'التخصيص حسب وثائق التعمير',
      'الاستعمال الفعلي', ' القيمة التداولية للعقـار', 'الاستغلال الخصوصي من طرف الغير',
      '  تاريخ إبرام عقد الكراء أو الاستغلال أو اتفاقية الوضع رهن الاشارة', 'الشخص المكتري', 'مدة الكراء', 'ثمن الكراء',
      'نسبة الزيادة', 'تاريخ اداء الكراء', 'مرجع وتاريخ سند إخراج العقار من الملك الخاص ',
      'مبرر إخراج العقار', 'مرجع و تاريخ خروج العقار في حالة ترتيبه في  الملك العام أو تفويته أو مبادلته', 'ثمن التفويت أو مبلغ المدرك في حالة المبادلة',
      'ملاحظات'
    ];

    // En-tête du tableau
    const headerRow = new TableRow({
      children: headers.reverse().map(h =>
        new TableCell({
          children: [new Paragraph({ text: h, alignment: AlignmentType.RIGHT })],
          verticalAlign: VerticalAlign.CENTER,
          borders: {
            top: { style: BorderStyle.SINGLE },
            bottom: { style: BorderStyle.SINGLE },
            left: { style: BorderStyle.SINGLE },
            right: { style: BorderStyle.SINGLE },
          },
          shading: { fill: 'D9D9D9' }
        })
      )
    });


    // Lignes de données
    const dataRows = privatePats.map(p => new TableRow({
      children: [
                new TableCell({ children: [new Paragraph(String(p.notesAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.transferPriceOrExchangeAmount ?? ''))] }),
        new TableCell({ children: [new Paragraph(`${p.transferOrPublicDomainReference ?? ''} | ${p.transferOrPublicDomainDate ?? ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.privateDomainRemovalJustificationAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(`${p.privateDomainRemovalReference ?? ''} | ${p.privateDomainRemovalDate ?? ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.rentPaymentDate ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.increaseRatePercent ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.rentalPrice ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.leaseDuration ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.tenantNameAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.leaseAgreementDate ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.privateUseDetailsAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.marketValue ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.currentUseAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.zoningDesignationAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.purchasePrice ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.acquisitionSourceAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(`العرض: ${p.latitude ?? ''}, الطول: ${p.longitude ?? ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.locationAr ?? ''))] }),
        new TableCell({ children: [new Paragraph(String(p.area ?? ''))] }),
        new TableCell({ children: [new Paragraph(`${p.registrationReference ?? ''} | ${p.registrationDate ?? ''}`)] }),
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
            new Paragraph({ text: 'قائمة الأملاك الخاصة', alignment: AlignmentType.CENTER}),
            new Paragraph({ text: '' }),
            table
          ]
        }
      ]
    });

    Packer.toBlob(doc).then(blob => saveAs(blob, 'قائمة_الأملاك_الخاصة.docx'));
  }
}
