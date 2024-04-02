import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'levelDescription'
})
export class LevelDescriptionPipe implements PipeTransform {
  transform(languages: { name: string, level: number }[]): string {
    // Sort languages by level
    languages.sort((a, b) => b.level - a.level);

    // Map levels to descriptions
    return languages.map(language => `${language.name} (${this.mapLevel(language.level)})`).join(', ');
  }

  private mapLevel(level: number): string {
    switch (level) {
      case 1:
        return 'Beginner';
      case 2:
        return 'Medium';
      case 3:
        return 'Advanced';
      case 4:
        return 'Native';
      default:
        return 'Unknown';
    }
  }
}