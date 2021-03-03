import { TestBed } from '@angular/core/testing';

import { FilesTableService } from './files-table.service';

describe('TableTemplateService', () => {
  let service: FilesTableService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FilesTableService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
