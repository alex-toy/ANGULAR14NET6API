import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CreateLevelDto } from 'src/app/models/levels/createLevelDto';
import { ResponseDto } from 'src/app/models/responseDto';
import { LevelService } from 'src/app/services/level.service';

@Component({
  selector: 'app-level-modal',
  templateUrl: './add-level-modal.component.html',
  styleUrls: ['./add-level-modal.component.css']
})
export class LevelModalComponent {
  @Input() dimensionId: number | null = null;
  @Input() ancestorId: number | null = null;
  @Input() showModal: boolean = false; 
  @Output() levelAdded = new EventEmitter<CreateLevelDto>();
  newLevel: CreateLevelDto = { label: '', dimensionId: 0, ancestorId: null };

  constructor(private levelService: LevelService) { }

  ngOnInit() {
    if (this.dimensionId) {
      this.newLevel.dimensionId = this.dimensionId; 
    }
  }

  closeModal() {
    this.showModal = false;  // Close the modal
  }

  addLevel() {
    if (!this.dimensionId) return;

    this.newLevel.dimensionId = this.dimensionId; 
    this.newLevel.ancestorId = this.ancestorId; 

    this.levelService.createLevel(this.newLevel).subscribe({
      next: (response: ResponseDto<number>) => {
        this.levelAdded.emit(this.newLevel);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating level:', err);
      }
    });
  }
}
