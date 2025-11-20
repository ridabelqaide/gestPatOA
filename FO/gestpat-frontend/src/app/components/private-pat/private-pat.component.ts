import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PrivatePat } from '../../models/private-pat.model';
import { PrivatePatService } from '../../Services/private-pat.service';
import { Document, Packer, Paragraph, Table, TableCell, TableRow, AlignmentType, BorderStyle, VerticalAlign } from "docx";
import { saveAs } from "file-saver";
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatPaginatorIntl } from '@angular/material/paginator';

@Component({
  selector: 'app-private-pat',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './private-pat.component.html',
  styleUrls: ['./private-pat.component.css']
})
export class PrivatePatComponent implements OnInit {

  privatePats: PrivatePat[] = [];
  selectedPat: PrivatePat | null = null;
  searchMatricule: string = '';
  isFormModalOpen = false;
  isDetailModalOpen = false;
  isEditMode = false;

  page = 1;
  pageSize = 5;
  totalItems = 0;

  typeAr: string = '';
  locationAr: string = '';
  types: string[] = [
    'فلاحي','سكني','تجاري',
    'صناعي','غابوي',
    'أرض عارية','أرض مجهزة',
    'أرض فلاحية سقوية','أرض فلاحية بورية'
  ];
  locations: string[] = [
    'فقرة','م. الفاسي','أولاد عزوز',
    'لقفاف','گوفاف',
    'بني يخلف','أولاد محمد',
    'أولاد فارس', "جمعة"
  ];

  formData: PrivatePat = this.getEmptyFormData();

  constructor(private privatePatService: PrivatePatService, private toastr: ToastrService, private paginatorIntl: MatPaginatorIntl) {
    this.paginatorIntl.itemsPerPageLabel = 'عدد العناصر لكل صفحة';
    this.paginatorIntl.nextPageLabel = 'التالي';
    this.paginatorIntl.previousPageLabel = 'السابق';
    this.paginatorIntl.firstPageLabel = 'الأول';
    this.paginatorIntl.lastPageLabel = 'الأخير';
   }

  ngOnInit(): void {
    this.loadPrivatePats();
  }

  private getEmptyFormData(): PrivatePat {
    return {
      registrationNumber: '',
      typeAr: '',
      landReferencesAr: '',
      registrationReference: '',
      registrationDate: new Date(),
      area: 0,
      locationAr: '',
      latitude: 0,
      longitude: 0,
      acquisitionSourceAr: '',
      purchasePrice: 0,
      zoningDesignationAr: '',
      currentUseAr: '',
      marketValue: 0,
      privateUseDetailsAr: '',
      leaseAgreementDate: new Date(),
      tenantNameAr: '',
      leaseDuration: 0,
      rentalPrice: 0,
      increaseRatePercent: 0,
      rentPaymentDate: new Date(),
      privateDomainRemovalReference: '',
      privateDomainRemovalDate: new Date(),
      privateDomainRemovalJustificationAr: '',
      transferOrPublicDomainReference: '',
      transferOrPublicDomainDate: new Date(),
      transferPriceOrExchangeAmount: 0,
      notesAr: ''
    };
  }

  loadPrivatePats(search?: string): void {
    this.privatePatService.getPagedPrivatePats(
      this.page,
      this.pageSize,
      this.searchMatricule?.trim() || '', 
      this.typeAr,                        
      this.locationAr                     
    ).subscribe({
      next: (res) => {
        this.privatePats = res.data;
        this.totalItems = res.totalItems;
      },
      error: (err) => console.error(err)
    });
  }


  onPageChange(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadPrivatePats();
  }

  applyFilters(): void {
    this.page = 1;
    this.loadPrivatePats(); 
  }

  onSearch(): void {
    this.page = 1;
    this.loadPrivatePats(this.searchMatricule?.trim() || ''); 
  }


  resetFilters(): void {
    this.typeAr = '';
    this.locationAr = '';
    this.searchMatricule = '';
    this.page = 1;
    this.loadPrivatePats();
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
      this.formData = this.getEmptyFormData();
      this.isEditMode = false;
    }
    this.isFormModalOpen = true;
  }

  closeForm() {
    this.isFormModalOpen = false;
    this.formData = this.getEmptyFormData();
  }

  submitForm(form: NgForm) {
    if (form.invalid) {
      Object.values(form.controls).forEach(c => c.markAsTouched());
      this.toastr.error('يرجى تصحيح الأخطاء في النموذج قبل الإرسال.', 'خطأ في النموذج');
      return;
    }

    if (this.isEditMode && this.formData.id) {
      this.privatePatService.update(this.formData.id, this.formData).subscribe({
        next: () => {
          this.loadPrivatePats();
          this.closeForm();
          this.toastr.success('تم التعديل بنجاح', 'التعديل');
        },
        error: (err) => {
          console.error('Erreur mise à jour PrivatePat:', err);
          this.toastr.error('حدث خطأ أثناء التعديل', 'خطأ');
        }
      });
    } else {
      const exists = this.privatePats.some(p => p.registrationNumber === this.formData.registrationNumber);
      if (exists) {
        this.toastr.error('هذا الرقم مسجل مسبقاً', 'خطأ');
        return;
      }

      this.privatePatService.create(this.formData).subscribe({
        next: () => {
          this.loadPrivatePats();
          this.closeForm();
          this.toastr.success('تم الإنشاء بنجاح', 'إنشاء');
        },
        error: (err) => {
          console.error('Erreur création PrivatePat:', err);
          this.toastr.error('حدث خطأ أثناء الإنشاء', 'خطأ');
        }
      });
    }
  }

  delete(p: PrivatePat) {
    if (!p.id) return;

    this.privatePatService.delete(p.id.toString()).subscribe({
      next: () => {
        this.loadPrivatePats();
        this.toastr.success('تم الحذف بنجاح', 'الحذف');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('حدث خطأ أثناء الحذف', 'خطأ');
      }
    });
  }

  exportToWord() {
    this.privatePatService.getAll().subscribe({
      next: (privatePats: PrivatePat[]) => {
        this.generateWordDoc(privatePats);
      },
      error: (err) => this.toastr.error('حدث خطأ أثناء جلب البيانات', 'خطأ')
    });
  }
  generateWordDoc(privatePats: PrivatePat[]) {
    if (!privatePats || privatePats.length === 0) return;

    const headers = [
      'رقم التقييد', 'النـــــــــــــوع', 'المراجع العقارية', 'المرجع و تاريخ التسجيل',
      'المساحة', 'الموقع', 'الإحداثيات', 'مصدر التملك', 'ثمن الاقتناء',
      'التخصيص حسب التعمير', 'الاستعمال الفعلي', 'القيمة التداولية', 'الاستغلال الخصوصي',
      'تاريخ عقد الكراء', 'المستأجر', 'مدة الكراء', 'ثمن الكراء', 'نسبة الزيادة', 'تاريخ أداء الكراء',
      'مرجع إخراج العقار', 'مبرر الإخراج', 'مرجع الخروج/التفويت', 'ثمن التفويت/المبادلة', 'ملاحظات'
    ];

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

    const dataRows = privatePats.map(p => new TableRow({
      children: [
        new TableCell({ children: [new Paragraph(p.notesAr || '')] }),
        new TableCell({ children: [new Paragraph(String(p.transferPriceOrExchangeAmount || ''))] }),
        new TableCell({ children: [new Paragraph(`${p.transferOrPublicDomainReference || ''} | ${p.transferOrPublicDomainDate || ''}`)] }),
        new TableCell({ children: [new Paragraph(p.privateDomainRemovalJustificationAr || '')] }),
        new TableCell({ children: [new Paragraph(`${p.privateDomainRemovalReference || ''} | ${p.privateDomainRemovalDate || ''}`)] }),
        new TableCell({ children: [new Paragraph(String(p.rentPaymentDate || ''))] }),
        new TableCell({ children: [new Paragraph(String(p.increaseRatePercent || ''))] }),
        new TableCell({ children: [new Paragraph(String(p.rentalPrice || ''))] }),
        new TableCell({ children: [new Paragraph(String(p.leaseDuration || ''))] }),
        new TableCell({ children: [new Paragraph(p.tenantNameAr || '')] }),
        new TableCell({ children: [new Paragraph(String(p.leaseAgreementDate || ''))] }),
        new TableCell({ children: [new Paragraph(p.privateUseDetailsAr || '')] }),
        new TableCell({ children: [new Paragraph(String(p.marketValue || ''))] }),
        new TableCell({ children: [new Paragraph(p.currentUseAr || '')] }),
        new TableCell({ children: [new Paragraph(p.zoningDesignationAr || '')] }),
        new TableCell({ children: [new Paragraph(String(p.purchasePrice || ''))] }),
        new TableCell({ children: [new Paragraph(p.acquisitionSourceAr || '')] }),
        new TableCell({ children: [new Paragraph(`العرض: ${p.latitude || ''}, الطول: ${p.longitude || ''}`)] }),
        new TableCell({ children: [new Paragraph(p.locationAr || '')] }),
        new TableCell({ children: [new Paragraph(String(p.area || ''))] }),
        new TableCell({ children: [new Paragraph(`${p.registrationReference || ''} | ${p.registrationDate || ''}`)] }),
        new TableCell({ children: [new Paragraph(p.landReferencesAr || '')] }),
        new TableCell({ children: [new Paragraph(p.typeAr || '')] }),
        new TableCell({ children: [new Paragraph(p.registrationNumber || '')] })
      ]
    }));

    const table = new Table({ rows: [headerRow, ...dataRows] });

    const doc = new Document({
      sections: [
        { children: [new Paragraph({ text: 'ملك جماعي- خاص -تابع للجماعة الترابية أولاد عبدون', alignment: AlignmentType.CENTER, spacing: { after: 400 } }), table] }
      ]
    });

    Packer.toBlob(doc).then(blob => saveAs(blob, 'قائمة_الأملاك_الخاصة.docx'));
    this.toastr.success('تم تصدير الملف بنجاح!', 'تصدير Word');
  }

}
