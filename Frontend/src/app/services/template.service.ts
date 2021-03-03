import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged.result';
import { File } from '../models/file';
import { TestTemplate } from '../models/test.template';
import { AbstractService } from './abstract.service';

@Injectable({
  providedIn: 'root'
})
export class FileService extends AbstractService<File> {

  constructor(http: HttpClient) {
    super('File', http)
  }
}
