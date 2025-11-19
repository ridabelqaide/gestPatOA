import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule,NgForm } from '@angular/forms';
import { PublicPatService } from '../../Services/public-pat.service';
import { PublicPat } from '../../models/public-pat.model';
import { Document, Packer, Paragraph, Table, TableCell, TableRow, TextRun, AlignmentType, BorderStyle, VerticalAlign } from "docx";
import { saveAs } from "file-saver";
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { MatPaginatorIntl } from '@angular/material/paginator';

@Component({
  selector: 'app-public-pat',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
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
  page = 1;
  pageSize = 5;
  totalItems = 0;

  typeAr: string = '';
  locationAr: string = '';
  types: string[] = [
    'فلاحي', 'سكني', 'تجاري',
    'صناعي', 'غابوي',
    'أرض عارية', 'أرض مجهزة',
    'أرض فلاحية سقوية', 'أرض فلاحية بورية'
  ];
  locations: string[] = [
    'فقرة', 'م. الفاسي', 'أولاد عزوز',
    'لقفاف', 'گوفاف',
    'بني يخلف', 'أولاد محمد',
    'أولاد فارس', "جمعة"
  ];

  constructor(private publicPatService: PublicPatService, private toastr: ToastrService, private paginatorIntl: MatPaginatorIntl) {
    this.paginatorIntl.itemsPerPageLabel = 'عدد العناصر لكل صفحة';
    this.paginatorIntl.nextPageLabel = 'التالي';
    this.paginatorIntl.previousPageLabel = 'السابق';
    this.paginatorIntl.firstPageLabel = 'الأول';
    this.paginatorIntl.lastPageLabel = 'الأخير';
  }

  ngOnInit(): void {
    this.loadPublicPats();
  }
  loadPublicPats(search?: string): void {
    this.publicPatService.getPagedPublicPats(
      this.page,
      this.pageSize,
      this.searchMatricule?.trim() || '',
      this.typeAr,
      this.locationAr
    ).subscribe({
      next: (res) => {
        this.publicPats = res.data;
        this.totalItems = res.totalItems;
      },
      error: (err) => console.error(err)
    });
  }
  onPageChange(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadPublicPats();
  }

  applyFilters(): void {
    this.page = 1;
    this.loadPublicPats();
  }

  onSearch(): void {
    this.page = 1;
    this.loadPublicPats(this.searchMatricule?.trim() || '');
  }


  resetFilters(): void {
    this.typeAr = '';
    this.locationAr = '';
    this.searchMatricule = '';
    this.page = 1;
    this.loadPublicPats();
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

  save(form: NgForm) {
    if (form.invalid) {
      Object.values(form.controls).forEach(c => c.markAsTouched());
      this.toastr.error('يرجى تصحيح الأخطاء في النموذج قبل الإرسال.', 'خطأ في النموذج');
      return;
    }

    if (this.isEditMode && this.formData.id) {
      this.publicPatService.update(this.formData.id, this.formData).subscribe({
        next: () => {
          this.loadPublicPats();
          this.closeForm();
          this.toastr.success('تم التعديل بنجاح', 'التعديل');
        },
        error: (err) => {
          console.error('Erreur mise à jour PublicPat:', err);
          this.toastr.error('حدث خطأ أثناء التعديل', 'خطأ');
        }
      });
    } else {
      const exists = this.publicPats.some(p => p.registrationNumber === this.formData.registrationNumber);
      if (exists) {
        this.toastr.error('هذا الرقم مسجل مسبقاً', 'خطأ');
        return;
      }

      this.publicPatService.create(this.formData).subscribe({
        next: (response) => {
          console.log('✔ PublicPat créé avec succès :', response); 
          this.loadPublicPats();
          this.closeForm();
          this.toastr.success('تم الإنشاء بنجاح', 'إنشاء');
        },
        error: (err) => {
          console.error('Erreur création PublicPat:', err);
          this.toastr.error('حدث خطأ أثناء الإنشاء', 'خطأ');
        }
      });
    }
  }
 
  delete(p: PublicPat) {
    if (!p.id) return;

    this.publicPatService.delete(p.id.toString()).subscribe({
      next: () => {
        this.loadPublicPats();
        this.toastr.success('تم الحذف بنجاح', 'الحذف');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('حدث خطأ أثناء الحذف', 'خطأ');
      }
    });
  }

  exportToWord() {
    this.publicPatService.getAll().subscribe({
      next: (publicPats: PublicPat[]) => {
        this.generateWordDoc(publicPats);
      },
      error: (err) => this.toastr.error('حدث خطأ أثناء جلب البيانات', 'خطأ')
    });
  }
  generateWordDoc(publicPats: PublicPat[]) {
    if (!publicPats || publicPats.length === 0) return;

    const headers = [
      'ملاحظات','تاريخ إخراج العقار','مرجع إخراج العقار من الملك العام (في حالة الإخراج)',
      'أجال و كيفيات أداء إتاوة الاحتلال المؤقت','نسبة الزيادة','مبلغ إتاوة الاحتلال','مدة إتاوة الاحتلال',
      'مدة الاحتلال المؤقت', 'الشخص المرخص له بالاحتلال المؤقت(أو المرخص لهم)','تاريخ إبرام سند الترخيص بالاستغلال:',
      'الاستغلال الخصوصي من طرف الغير','القيمة التداولية للعقـار','الاستعمال الفعلي','التخصيص حسب وثائق التعمير',
      'الأساس القانوني المعتمد لترتيب العقار ضمن الأملاك العامة','ثمن الاقتناء', 'مصدر تملك العقــار','الإحداثيات الجغرافية', 'الموقع',
      'المساحة','المرجع و تاريخ تسجيله بإدارة التسجيل','المراجع العقارية','النـــــــــــــوع','رقم التقييد في السجل'
    ];


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
              children: [new TextRun({ text: 'ملك جماعي- عام -تابع للجماعة الترابية أولاد عبدون', bold: true, size: 48 })],
              alignment: AlignmentType.CENTER
            }),
            new Paragraph({ text: '' }),
            table
          ]
        }
      ]
    });

    Packer.toBlob(doc).then(blob => saveAs(blob, 'قائمة_الأملاك_العامة.docx'));
    this.toastr.success('تم تصدير الملف بنجاح!', 'تصدير Word');

  }
}
