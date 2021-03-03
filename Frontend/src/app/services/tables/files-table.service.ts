import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { File } from 'src/app/models/file';
import { FileService } from '../template.service';
import { AbstractTableService } from './abstract-table.service';

@Injectable({
  providedIn: 'root'
})
export class FilesTableService extends AbstractTableService<File> {

  edit(id: number): void {
    this.router.navigate(['/main/template', id])
  }

  public get pageSizeOptions(): number[] {
    return [5, 10, 25, 100];
  }
  
  public get displayedColumns(): string[] {
    return ['name', 'description', 'actions']
  }

  constructor(service: FileService, toastr: ToastrService, router: Router, dialog: MatDialog) {
    super(service, toastr, router, dialog)
  }
}
