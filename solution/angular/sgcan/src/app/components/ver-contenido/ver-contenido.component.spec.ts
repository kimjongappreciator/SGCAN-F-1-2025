import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerContenidoComponent } from './ver-contenido.component';

describe('VerContenidoComponent', () => {
  let component: VerContenidoComponent;
  let fixture: ComponentFixture<VerContenidoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerContenidoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerContenidoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
